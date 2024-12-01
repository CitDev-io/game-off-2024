using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardManager : MonoBehaviour
{
    public GameObject DotPrefab;
    public GameObject UnityTilePrefab;
    public Image InHandTileImg;
    public CoreCartridge GridGameInstance;
    
    public Tile TemporarilyGlobalTileInHand;
    public UI_UnityTile StagedTile;
    int Confirmations = 0;
    Dictionary<GridPosition, UI_DotSpot> ClickGrid = new Dictionary<GridPosition, UI_DotSpot>();
    public GameObject TilePlacementUserInput;
    Coroutine CameraOperator;
    public PlayerSlot CurrentPlayer = PlayerSlot.NEUTRAL;
    public List<UI_UnityTile> Tiles = new();
    public CanvasGroup wired_UI_SecretPanel;
    public CanvasGroup wired_UI_CaptainPanel;
    public CanvasGroup wired_UI_AnnouncementPanel;
    public CanvasGroup wired_UI_PlaytimePanel;
    public CanvasGroup wired_UI_EndOfGameButtonsPanel;

    public float DEFAULT_CAMERA_FOV = 45f;
    public float ZOOMED_CAMERA_FOV = 25f;
    public float ZOOMED_WAY_OUT_CAMERA_FOV = 70f;
    public float AUTO_ZOOM_SPEED = 2.5f;
    public float AUTO_PAN_SPEED = 3f;
    public float AUTO_PAN_SNAP_DISTANCE = 0.1f;
    public float AUTO_ZOOM_SNAP_DISTANCE = 0.1f;

    public bool LookupTFEligibilityForTileAndGroupId(Tile t, int groupId) {
        List<GamepieceTileAssignment> GTAs = GridGameInstance.inventory.FindAllGTAsFromTileAndGroupId(t, groupId);
        return GTAs.All(gta => gta.Type != GamepieceType.TERRAFORMER);
    }

    void Start()
    {
        BootSequence();
    }

    void OnDestroy() {
        EndGameBridge();
    }

    void StartGameBridge() {
        GridGameInstance.OnPlayerTurnChange += StartTurnPerformanceForPlayer;
    }

    void EndGameBridge() {
        GridGameInstance.OnPlayerTurnChange -= StartTurnPerformanceForPlayer;
    }

    void BootSequence() {
        GameSettings settings = new GameSettings();
        InitCoreCartridge(settings);
        InitUnityCartridge(settings);
        StartGameBridge();

        // game intro stuff
        // setup scoreboard etc

        StartCoroutine(OpeningSequence());
    }
    IEnumerator OpeningSequence() {
        CurrentPlayer = GridGameInstance.scoreboard.GetCurrentTurnPlayer().slot;
        DisableTileHighlightsForCurrentPlayer();
        _ui_UpdateObjectiveBoard();
        UpdateDrawnTile();
        UpdateClickGrid();
        wired_UI_SecretPanel.alpha = 0;
        wired_UI_PlaytimePanel.alpha = 0;
        CameraControlTo(
            new Vector3(0, 0, -8),
            ZOOMED_WAY_OUT_CAMERA_FOV
        );
        yield return StartCoroutine(CaptainOpening());
        wired_UI_SecretPanel.alpha = 1;
        wired_UI_PlaytimePanel.alpha = 1;
        CameraControlTo(
            new Vector3(0, 0, -8),
            DEFAULT_CAMERA_FOV
        );
        StartTurnPerformanceForPlayer(GridGameInstance.scoreboard.GetCurrentTurnPlayer());
    }

    void _ui_UpdateObjectiveBoard() {
        FindAnyObjectByType<UI_ObjectiveOverlayManager>().UpdateObjectivesForPlayer(PlayerSlot.PLAYER1);
    }
    void DisableTileHighlightsForCurrentPlayer() {
        Tiles
            .Where(t => t.LastPlacedIndicator.sprite.name == (CurrentPlayer == PlayerSlot.PLAYER1 ? "TileHighlightBlue" : "TileHighlightPink"))
            .ToList()
            .ForEach(i => i.DisableLastPlacementGlow());
    }
    void StartTurnPerformanceForPlayer(PlayerAssignment pa) {
        // now's your chance. here's who's turn is starting right now
        CurrentPlayer = pa.slot;
        DisableTileHighlightsForCurrentPlayer();
        _ui_UpdateObjectiveBoard();

        bool GameIsOver = GridGameInstance.IsGameOver();
        if (GameIsOver) {
            StartCoroutine(BRIDGE_DoEndOfGameSequence());
            return;
        }

        Debug.Log("STARTING TURN FOR PLAYER: " + pa.slot + " - " + pa.type);

        UpdateDrawnTile();
        
        if (pa.type == PlayerType.CPU) {
            StartCoroutine(DOCPUTURN());
        }
    }

    IEnumerator DOCPUTURN() {
        // yield return new WaitForSeconds(1.5f);
        // TripAckCheck();
        yield return new WaitForSeconds(3f);
        // var randomizedSpots = FindObjectsOfType<UI_DotSpot>()
        //     .Where(d => d.visibility)
        //     .OrderBy(d => Guid.NewGuid())
        //     .ToList();

        var populatedSpots = FindObjectsOfType<UI_DotSpot>()
            .Where(d => d.visibility)
            .OrderByDescending(d => GridGameInstance.SurveyPosition(d.coords).ExtendedNeighborhoodCount())
            .ToList();
        GridPosition pos = populatedSpots.First().coords;


        AI_TileSpotClick(pos);
        yield return new WaitForSeconds(1.5f);
        UIINGRESS_OnPlayerAccept();
        yield return new WaitForSeconds(1.5f);
        if (Confirmations == 1) {
            // TODO: Stop picking farms so damn much
            List<UI_TileGPDropZone> farmlessDropZones = FindObjectsOfType<UI_TileGPDropZone>()
                .Where(d => TemporarilyGlobalTileInHand.GetTypeOfAnchorId(d.GetAnchorId()) != EdgeType.FARM)
                .ToList();

            if (farmlessDropZones.Count() > 0) {
                int anchorId = farmlessDropZones
                    .OrderBy(d => Guid.NewGuid())
                    .First().GetAnchorId();
                AI_AssignTerraformerToAnchor((int)anchorId);
                yield return new WaitForSeconds(1f);
            }

            UIINGRESS_OnPlayerAccept();
        }
        yield return new WaitForSeconds(1.5f);
        // yield return new WaitForSeconds(3.5f);
        // Debug.Log("READY TO ACK?");
        // // GridPosition coords = GridGameInstance.GetBestMoveForCPU();
        // // StageUnityTileAt(TemporarilyGlobalTileInHand, coords);
        // GridGameInstance.Ack();
    }

    void UpdateDrawnTile() {
        Tile newTile = GridGameInstance.CurrentTile;

        TemporarilyGlobalTileInHand = newTile;
        UpdateClickGrid();
        InHandTileImg.sprite = Resources.Load<Sprite>("Images/Tiles/Tile_" + newTile.Name);
        InHandTileImg.transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, -90 * newTile.Rotation)
        );
    }

    void UpdateClickGrid() {
        ClearClickGrid();

        foreach (GridPosition pos in GridGameInstance.GetEligiblePositionsAllRotations(TemporarilyGlobalTileInHand)) {
            ClickGrid[pos].Enable();
        }
    }

    void ClearClickGrid() {
        foreach (UI_DotSpot dot in ClickGrid.Values) {
            dot.Disable();
        }
    }

    List<int> GetWorkableRotations(Tile tile, GridPosition coords) {
        List<int> workableRotations = new List<int>();
        for (int i = 0; i < 4; i++) {
            Tile checkTile = TileFactory.CreateTileWithRotations(
                (TileType) Enum.Parse(typeof(TileType), tile.Name),
                i
            );
            if (GridGameInstance.CanPlaceTile(checkTile, coords)) {
                workableRotations.Add(i);
            }
        }
        return workableRotations;
    }

    UI_UnityTile StageUnityTileAt(Tile tile, GridPosition coords) {
        ClearClickGrid();
        List<int> Workables = GetWorkableRotations(tile, coords);
        Tile tileToDrop = TileFactory.CreateTileWithRotations(
            (TileType) Enum.Parse(typeof(TileType), tile.Name),
            Workables.Count > 0 ? Workables[0] : 0
        );
        StagedTile = Instantiate(UnityTilePrefab).GetComponent<UI_UnityTile>();
        StagedTile.RegisterTile(
            tileToDrop,
            coords,
            Workables,
            GridGameInstance.SurveyPosition(coords)
        );
        Tiles.Add(StagedTile);
        TripAckCheck();
        return StagedTile;
    }

    void ClearStagingUI() {
        TilePlacementUserInput.SetActive(false);
    }

    void InitCoreCartridge(GameSettings settings) {
        
        GridGameInstance = new CoreCartridge(
            TileFactory.CreateTile(TileType.D),
            settings
        );
    }

    void InitUnityCartridge(GameSettings settings) {
        var StarterTile = StageUnityTileAt(GridGameInstance.grid[settings.BoardMidPoint, settings.BoardMidPoint], new GridPosition(settings.BoardMidPoint, settings.BoardMidPoint));
        StarterTile.AllowPlacementglow = false;
        Tiles.Add(StarterTile);
        StarterTile.SetStatus(UITileStatus.PLACED, PlayerSlot.PLAYER1);

        ClearStagingUI();
        for (int x = 0; x < settings.OddGameBoardWidth; x++) {
            for (int y = 0; y < settings.OddGameBoardWidth; y++) {
                GameObject dot = Instantiate(DotPrefab, new Vector3(x - settings.BoardMidPoint, y - settings.BoardMidPoint, 0.2f), Quaternion.Euler(-90f, 0, 0));
                dot.transform.parent = transform;

                GridPosition coords = new GridPosition(x, y);
                UI_DotSpot dotSpot = dot.GetComponent<UI_DotSpot>();
                dotSpot.SetCoords(coords);
                ClickGrid[coords] = dotSpot;
            }
        }
    }

    void CameraControlTo(Vector3 target, float cameraFov) {
        if (CameraOperator != null) {
            StopCoroutine(CameraOperator);
        }
        CameraOperator = StartCoroutine(RoutineCameraControl(target, cameraFov));
    }

    IEnumerator RoutineCameraControl(Vector3 target, float cameraFov) {
        Camera.main.GetComponent<DragCamera>().DisableUserInput = true;
        while (Vector3.Distance(Camera.main.transform.position, target) > AUTO_PAN_SNAP_DISTANCE || Mathf.Abs(Camera.main.fieldOfView - cameraFov) > AUTO_ZOOM_SNAP_DISTANCE) {
            Vector3 lerpSpot = Vector3.Lerp(
                Camera.main.transform.position,
                target,
                Time.deltaTime * AUTO_PAN_SPEED
            );
            Camera.main.transform.position = new Vector3(
                lerpSpot.x,
                lerpSpot.y,
                Camera.main.transform.position.z
            );
            
            Camera.main.fieldOfView = Mathf.Lerp(
                Camera.main.fieldOfView,
                cameraFov,
                Time.deltaTime * AUTO_ZOOM_SPEED
            );
            yield return null;
        }
        Camera.main.transform.position = new Vector3(
            target.x,
            target.y,
            Camera.main.transform.position.z
        );
        Camera.main.fieldOfView = cameraFov;
        Camera.main.GetComponent<DragCamera>().DisableUserInput = false;
    }

    void TripAckCheck() {
        ClearStagingUI();
        if (Confirmations == 0) {
            TilePlacementUserInput.SetActive(GridGameInstance.scoreboard.GetCurrentTurnPlayer().type == PlayerType.HUMAN);
            StagedTile.SetStatus(UITileStatus.CONFIGURE_TRANSFORM, GridGameInstance.scoreboard.GetCurrentTurnPlayer().slot);
            CameraControlTo(
                new Vector3(
                    StagedTile.transform.position.x,
                    StagedTile.transform.position.y,
                    -8)
                , DEFAULT_CAMERA_FOV
            );
        }
        if (Confirmations == 1) {
            if (GridGameInstance.scoreboard.Stats[
                GridGameInstance.scoreboard.GetCurrentTurnPlayer().slot
            ].TerraformerCount > 0
            ) {
                TilePlacementUserInput.SetActive(GridGameInstance.scoreboard.GetCurrentTurnPlayer().type == PlayerType.HUMAN);
                StagedTile.CancelGamepiecePlacement();

                StagedTile.SetStatus(UITileStatus.CONFIGURE_TERRAFORMER, GridGameInstance.scoreboard.GetCurrentTurnPlayer().slot);
                CameraControlTo(
                    new Vector3(
                        StagedTile.transform.position.x,
                        StagedTile.transform.position.y,
                        -8),
                    ZOOMED_CAMERA_FOV
                );
            } else {
                Confirmations++;
         }
            // ************* if not, skip to the next step
        }
        if (Confirmations == 2 && StagedTile.GetStatus() != UITileStatus.PLACED) {
            GridGameInstance
                .PlaceTile(
                    StagedTile.registeredTile,
                    StagedTile.gridPosition
                );

            StagedTile.SetStatus(UITileStatus.PLACED, GridGameInstance.scoreboard.GetCurrentTurnPlayer().slot);

            Action OnPerformanceComplete = () => {
                GridGameInstance.Ack();
                Confirmations = 0;
                CameraControlTo(Camera.main.transform.position, DEFAULT_CAMERA_FOV);
            };
            StartCoroutine(BRIDGE_DoEndOfTurnSequence(OnPerformanceComplete));
        }
    }

    IEnumerator BRIDGE_DoEndOfGameSequence() {
        List<ScoringEvent> SecretObjectiveScoringEvents = GridGameInstance.GetScoringEvents_ObjectiveCheck();
        yield return StartCoroutine(ProcessAndInvokeScoringEvents(SecretObjectiveScoringEvents));

        // List<ScoringEvent> EndOfGameScoringEvents = GridGameInstance.GetScoringEvents_EndGame();
        // yield return StartCoroutine(ProcessAndInvokeScoringEvents(EndOfGameScoringEvents, true));

        // cancel out of any turn that might be active
        CancelStagingInput();
        // hide objectives and control panel and tile in hand
        wired_UI_SecretPanel.alpha = 0;
        wired_UI_PlaytimePanel.alpha = 0;
        wired_UI_CaptainPanel.alpha = 0;
        wired_UI_AnnouncementPanel.alpha = 0;

        FindFirstObjectByType<UI_EOGOverlayManager>().Present();
        wired_UI_EndOfGameButtonsPanel.alpha = 1;
        wired_UI_EndOfGameButtonsPanel.interactable = true;
        wired_UI_EndOfGameButtonsPanel.blocksRaycasts = true;
    }

    public void INS_DoEOG() {
        StartCoroutine(BRIDGE_DoEndOfGameSequence());
    }

    IEnumerator BRIDGE_DoEndOfTurnSequence(Action UIOnComplete) {
        // board scoring events
        List<ScoringEvent> BoardScoringEvents = GridGameInstance.GetScoringEvents_TilePlaced();
        yield return StartCoroutine(ProcessAndInvokeScoringEvents(BoardScoringEvents));

        // secret objective scoring events
        List<ScoringEvent> SecretObjectiveScoringEvents = GridGameInstance.GetScoringEvents_ObjectiveCheck();
        yield return StartCoroutine(ProcessAndInvokeScoringEvents(SecretObjectiveScoringEvents));

        // scoring events for role/rank
        List<ScoringEvent> RankScoringEvents = GridGameInstance.GetScoringEvents_RoleProgress();
        bool RankUpForPlayer = RankScoringEvents.Count > 0 && RankScoringEvents.Any(e => e.PrivacyFilter == PlayerSlot.PLAYER1);
        if (RankUpForPlayer) {
            wired_UI_SecretPanel.alpha = 0;
            wired_UI_PlaytimePanel.alpha = 0;
            CameraControlTo(
                new Vector3(0, 0, -8),
                ZOOMED_WAY_OUT_CAMERA_FOV
            );
        }
        yield return StartCoroutine(ProcessAndInvokeScoringEvents(RankScoringEvents));
        if (RankUpForPlayer) {
            wired_UI_SecretPanel.alpha = 1;
            wired_UI_PlaytimePanel.alpha = 1;
            CameraControlTo(
                new Vector3(0, 0, -8),
                DEFAULT_CAMERA_FOV
            );
        }

        UIOnComplete.Invoke();
    }

    IEnumerator ProcessAndInvokeScoringEvents(List<ScoringEvent> events) {
        List<ScoringEvent> eventsToPerform = events
            .Where(e => e.PrivacyFilter == PlayerSlot.NEUTRAL || e.PrivacyFilter == PlayerSlot.PLAYER1)
            .ToList();
        foreach(ScoringEvent e in eventsToPerform) {
            if (eventsToPerform.Contains(e)) {
                Debug.Log("PROCESSING " + e.EventType + " EVENT:");
                Debug.Log(e.Description);

                switch(e.EventType) {
                    case ScoringEventType.ROADCOMPLETED:
                    case ScoringEventType.CITYCOMPLETED:
                    case ScoringEventType.FARMSCORED:
                    case ScoringEventType.OBELISKCOMPLETED:
                        yield return StartCoroutine(PerformScoringEvent_RoadCityObelisk(e));
                        break;
                    case ScoringEventType.PROMOTION:
                        yield return StartCoroutine(CaptainsQuarters(e));
                        break;
                    case ScoringEventType.INCOMPLETE:
                        continue;
                    default:
                        break;
                }

                if (e.Description != "" && e.Description != null && e.EventType != ScoringEventType.INCOMPLETE) {
                    FindAnyObjectByType<UI_AnnouncementOverlayManager>().Announce(e.Description);
                }

            }
            yield return FindAnyObjectByType<UI_AnnouncementOverlayManager>().AwaitCrawlerComplete();
            e.ScoringAction.Invoke(); // COMMIT the scoring action
        }
        // yield return new WaitForSeconds(Mathf.Min(1.5f * eventsToPerform.Count, 3f));
    }

    IEnumerator CaptainOpening() {
        string message = "The alliance to settle a new home world has...totally collapsed (whoops!).\n\n";
        message += "Now, rival factions are racing to shape the planet’s future, one Terra Tile at a time, each with their own secret agenda.\n\n";
        message += "Choose your side wisely, terraform your tiles cleverly, and try not to ruin another perfectly habitable planet. Our future is in your hands (no pressure).";
        yield return FindAnyObjectByType<UI_CaptainOverlayManager>().Announce(message, 8);

        yield return new WaitForSeconds(0.5f); // captain script still chillin.. if he wants to do anything else... pausing....
    }
    IEnumerator CaptainsQuarters(ScoringEvent e) {
        string message = "CONGRATULATIONS!\n\nYou've been promoted to "; //RANK 1\nDIRTLING (DRT)\n\nYou really showed initiative out there!";
        switch(GridGameInstance.scoreboard.Stats[e.PrivacyFilter].Rank) {
            case SecretObjectiveRank.RECRUIT:
                message += "RANK 1\nDIRTLING (DRT)"; //\n\nYou really showed initiative out there!";
                break;
            case SecretObjectiveRank.DIRTLING:
                message += "RANK 2\nLANDSCRAPER (LSR)"; //\n\nYou really showed initiative out there!";
                break;
            case SecretObjectiveRank.LANDSCRAPER:
                message += "RANK 3\nSTARSHAPER (STR)"; //\n\nYou really showed initiative out there!";
                break;
            case SecretObjectiveRank.STARSHAPER:
                message += "RANK 4\nGALACTIC ENGINEER (GNG)"; //
                break;
        }
        message += "\n\nYou really showed initiative out there!";
        
        yield return FindAnyObjectByType<UI_CaptainOverlayManager>().Announce(message);

        yield return new WaitForSeconds(0.5f); // captain script still chillin.. if he wants to do anything else... pausing....
    }

    IEnumerator PerformScoringEvent_RoadCityObelisk(ScoringEvent e) {
        yield return null;
        e.RelatedGamepieces.ForEach(gp => {
            if (gp.Type == GamepieceType.TERRAFORMER) {
                List<UI_AnchorTag> ats = FindObjectsOfType<UI_AnchorTag>()
                .Where(anchor => anchor.AnchorId == gp.Anchor
                    && anchor.gridPosition == gp.Position
                ).ToList();
                foreach (UI_AnchorTag at in ats) {

                    // terraformer being recalled right here.
                    FindAnyObjectByType<GameController_DDOL>().PlaySound("Retrieve_Terraformer");
                    Destroy(at.gameObject);
                }
            }
        });
    }

    void CancelStagingInput() {
        Tiles.Remove(StagedTile);
        Destroy(StagedTile.gameObject);
        Confirmations = 0;
        StagedTile = null;
        UpdateClickGrid();
        ClearStagingUI();
    }

    public void OnUserCameraMovement() {
        if (CameraOperator != null)
            StopCoroutine(CameraOperator);
    }

    public void UIINGRESS_OnPlayerAccept() {
        if (GridGameInstance.IsGameOver()) return;
        if (StagedTile == null) return;
        Confirmations++;

        FindAnyObjectByType<GameController_DDOL>().PlaySound("Confirm_Selection");
        TripAckCheck();
    }

    public void UIINGRESS_OnPlayerBackStep() {
        if (GridGameInstance.IsGameOver()) return;
        if (StagedTile == null) return;
        Confirmations--;

        FindAnyObjectByType<GameController_DDOL>().PlaySound("Cancel_Tile");
        if (Confirmations > -1) {
            TripAckCheck();
        } else {
            CancelStagingInput();
        }
    }

    public void RotateTileInHand() {
        if (GridGameInstance.IsGameOver()) return;
        if (TemporarilyGlobalTileInHand == null) return;
        if (GridGameInstance.scoreboard.GetCurrentTurnPlayer().type != PlayerType.HUMAN) return;
        TemporarilyGlobalTileInHand.Rotate();

        InHandTileImg.transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, -90 * TemporarilyGlobalTileInHand.Rotation)
        );
    }

    public void RotateStagedTile() {
        if (GridGameInstance.IsGameOver()) return;
        if (StagedTile == null) return;
        if (GridGameInstance.scoreboard.GetCurrentTurnPlayer().type != PlayerType.HUMAN) return;
        StagedTile.RotateWorkableOnly();
    }

    public void OnDotClick(GridPosition coords)
    {
        if (GridGameInstance.IsGameOver()) return;
        if (GridGameInstance.scoreboard.GetCurrentTurnPlayer().type != PlayerType.HUMAN) return;

        if (TemporarilyGlobalTileInHand == null) return;

        FindAnyObjectByType<GameController_DDOL>().PlaySound("Confirm_Selection");
        StageUnityTileAt(TemporarilyGlobalTileInHand, coords);
    }

    public void AI_TileSpotClick(GridPosition coords) {
        if (TemporarilyGlobalTileInHand == null) return;

        FindAnyObjectByType<GameController_DDOL>().PlaySound("Confirm_Selection");
        StageUnityTileAt(TemporarilyGlobalTileInHand, coords);
    }

    public void AI_AssignTerraformerToAnchor(int anchorIndex) {
        if (StagedTile == null) return;
        
        StagedTile.AssignTerraformerToAnchorFacade(anchorIndex, GridGameInstance.scoreboard.GetCurrentTurnPlayer().slot);
    }

    public void UserAssignsTerraformerToAnchor(int anchorIndex) {
        if (GridGameInstance.scoreboard.GetCurrentTurnPlayer().type != PlayerType.HUMAN) return;
        if (StagedTile == null) return;
        StagedTile.AssignTerraformerToAnchorFacade(
            anchorIndex,
            GridGameInstance.scoreboard.GetCurrentTurnPlayer().slot
        );
    }
}
