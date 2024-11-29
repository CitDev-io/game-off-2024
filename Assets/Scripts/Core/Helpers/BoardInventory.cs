using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class OwnedFarm {
    public List<TileAndPlacementGroupIndex> tilePis = new List<TileAndPlacementGroupIndex>();
    public OwnedFarm() {}

    public void AddTilePlacementIndex(TileAndPlacementGroupIndex tpi) {
        tilePis.Add(tpi);
    }

    public void Merge(OwnedFarm of) {
        tilePis = tilePis.Union(of.tilePis).ToList();
    }

    public int GetUniqueTileCount() {
        return tilePis.Select(t => t.tile).Distinct().Count();
    }

    public bool IsTouchingACity() => tilePis.Any(tpi => tpi.tile.IsGroupTouchingCity(tpi.groupIndexId));
}

public class BoardInventory {
    public List<AssembledRoad> AssembledRoads = new List<AssembledRoad>();
    public List<AssembledObelisk> AssembledObelisks = new();
    public List<AssembledCity> AssembledCities = new();
    public List<OwnedFarm> OwnedFarms = new();

    public List<GamepieceTileAssignment> FindAllGTAsFromTileAndGroupId(Tile t, int groupId) {
        List<TileAndPlacementGroupIndex> theTPIsINeed = new();

        AssembledCity matchingAc = AssembledCities.Where(ac => ac
            .tilePis
            .Exists(tpi => tpi.groupIndexId == groupId && tpi.tile == t))
            .FirstOrDefault();

        if (matchingAc != null) {
            theTPIsINeed = matchingAc.tilePis;
            
            return theTPIsINeed
                .SelectMany(tpi => tpi.tile.GetAllGamepiecesInGroupId(tpi.groupIndexId))
                .ToList();
        }

        AssembledRoad matchingAr = AssembledRoads.Where(ar => ar.tilePis.Exists(tpi => tpi.groupIndexId == groupId && tpi.tile == t)).FirstOrDefault();
        if (matchingAr != null) {
            theTPIsINeed = matchingAr.tilePis;

            return theTPIsINeed
                .SelectMany(tpi => tpi.tile.GetAllGamepiecesInGroupId(tpi.groupIndexId))
                .ToList();
        }

        AssembledObelisk matchingOb = AssembledObelisks.Where(ao => ao.tilePi.groupIndexId == groupId && ao.tilePi.tile == t).FirstOrDefault();
        if (matchingOb != null) {
            theTPIsINeed = new List<TileAndPlacementGroupIndex>{ matchingOb.tilePi };

            return theTPIsINeed
                .SelectMany(tpi => tpi.tile.GetAllGamepiecesInGroupId(tpi.groupIndexId))
                .ToList();
        }

        OwnedFarm matchingOf = OwnedFarms.Where(of => of.tilePis.Exists(tpi => tpi.groupIndexId == groupId && tpi.tile == t)).FirstOrDefault();
        if (matchingOf != null) {
            theTPIsINeed = matchingOf.tilePis;

            return theTPIsINeed
                .SelectMany(tpi => tpi.tile.GetAllGamepiecesInGroupId(tpi.groupIndexId))
                .ToList();
        }

        return new List<GamepieceTileAssignment>();
    }

    public void AddTileToInventory(Tile t) {
        IndexRoadsForTile(t);
        IndexObelisksForTile(t);
        IndexCitiesForTile(t);
        IndexFarmsForTile(t);
    }

    void IndexFarmsForTile(Tile t) {
        // here are the groups to index
        List<int> FarmEdgeGroupIds = t.Edges
            .Where(e => e.type == MicroEdgeType.FARM)
            .Select(e => e.EdgeGroupId)
            .Distinct()
            .ToList();

        // for each group to index
        foreach(int farmEdgeGroupId in FarmEdgeGroupIds) {
            List<OwnedFarm> existingFarms = new();
            // collect up existing farms

            // we're just collecting up any existing farms that would be attached to this group's edge pxiels

            //foreach MicroEdge for this group
            List<CardinalDirection> dirsToCheck = t.GetCardinalDirectionsForGroupIndexId_Farms(farmEdgeGroupId);
 
            foreach(CardinalDirection dir in dirsToCheck) {
                Tile NeighborInDir = t.NormalizedSurvey.TileInDirection(dir);
                if (NeighborInDir == null) {
                    continue;
                }
                // now look through the two microedges in that direction
                // each of which that is represented in this group, specifically.
                List<MicroEdge> microEdgesInThisDirection = t.Edges
                    .Where(e => e.EdgeGroupId == farmEdgeGroupId && t.GetCardinalDirectionForRotatedEdge(e) == dir)
                    .ToList();

                foreach(MicroEdge me in microEdgesInThisDirection) {
                    // find the opposing microedge in the neighbor tile
                    // my spot id = meIndex
                    // i ultimatiely want to get whatever is currently in the opposite normalized spot id
                    int thisLocalizedIndex = (t.Edges.IndexOf(me) + (t.Rotation * 2))% 8;

                    // if (dir == CardinalDirection.NORTH && (thisLocalizedIndex != 0 && thisLocalizedIndex != 1)) {
                    //     // UnityEngine.Debug.Log("ok i'm not on the border to the north. nevermind.");
                    //     continue;
                    // }

                    // if (dir == CardinalDirection.WEST && (thisLocalizedIndex != 6 && thisLocalizedIndex != 7)) {
                    //     // UnityEngine.Debug.Log("ok i'm not on the border to the west. nevermind.");
                    //     continue;
                    // }

                    // if (dir == CardinalDirection.EAST && (thisLocalizedIndex != 2 && thisLocalizedIndex != 3)) {
                    //     // UnityEngine.Debug.Log("ok i'm not on the border to the east. nevermind.");
                    //     continue;
                    // }
                    // if (dir == CardinalDirection.SOUTH && (thisLocalizedIndex != 4 && thisLocalizedIndex != 5)) {
                    //     // UnityEngine.Debug.Log("ok i'm not on the border to the south. nevermind.");
                    //     continue;
                    // }



                    int thisLocalizedOpponentIndex = (thisLocalizedIndex + (thisLocalizedIndex % 2 == 0 ? 5 : 3)) % 8;
                    // remove rotations to normalize

                    int normalizedOppoSpotId = (thisLocalizedOpponentIndex + (NeighborInDir.Rotation * 6)) % 8;
                    // if oppo weren't rotated, we would just grab the ME at normalizedOppoSpotId
                    // int oppoLocalizedIndex = (normalizedOppoSpotId + (NeighborInDir.Rotation * 6)) % 8;
                    MicroEdge oppoSpot = NeighborInDir.Edges[normalizedOppoSpotId];
                    // find the group id of the opposing microedge
                    int oppoGroupId = oppoSpot.EdgeGroupId;
                    // see if any ownedfarms have this TPI alignment
                    OwnedFarm existingFarm = OwnedFarms
                        .Find(of => of.tilePis.Any(tpi => tpi.groupIndexId == oppoGroupId && tpi.tile == NeighborInDir));
                    // if they do, add them to the existing farm list
                    if (!existingFarms.Contains(existingFarm)) {
                        existingFarms.Add(existingFarm);
                    }
                }
            }

            OwnedFarm ownedFarm = new OwnedFarm();
            ownedFarm.AddTilePlacementIndex(
                new TileAndPlacementGroupIndex(
                    t,
                    farmEdgeGroupId
                )
            );
            foreach (OwnedFarm of in existingFarms.Distinct().ToList()) {
                ownedFarm.Merge(of);
                OwnedFarms.Remove(of);
            }

            OwnedFarms.Add(ownedFarm);
        }
    }

    void IndexCitiesForTile(Tile t) {
        List<int> CityEdgeGroupIds = t.Edges
            .Where(e => e.type == MicroEdgeType.CITY)
            .Select(e => e.EdgeGroupId)
            .Distinct()
            .ToList();

        foreach(int cityEdgeGroupId in CityEdgeGroupIds) {
            List<AssembledCity> existingCities = new();

            foreach (CardinalDirection dir in System.Enum.GetValues(typeof(CardinalDirection))) {
                if (t.GetEdgeTypeByNormalizedDir(dir) != EdgeType.CITY) {
                    continue;
                }

                MicroEdgeSpot spotToCheck = t.DecodeDirectionToTrueMicroEdgeSpot(dir);
                if (t.FindMicroEdgeFromLocalizedEdgeSpot(spotToCheck).EdgeGroupId != cityEdgeGroupId) {
                    continue;
                }
                
                if (t.NormalizedSurvey.TileInDirection(dir) != null) {
                    Tile neighborTile = t.NormalizedSurvey.TileInDirection(dir);
                    MicroEdgeSpot oppoSpot = neighborTile.DecodeDirectionToTrueMicroEdgeSpot(
                        GetOppositeDirection(dir)
                    );
                    int oppoGroupId = neighborTile.FindMicroEdgeFromLocalizedEdgeSpot(oppoSpot).EdgeGroupId;

                    AssembledCity existingCity = AssembledCities
                        .Find(ac => ac.tilePis.Any(tpi => tpi.groupIndexId == oppoGroupId && tpi.tile == neighborTile));

                    if (!existingCities.Contains(existingCity)) {
                        existingCities.Add(existingCity);
                    }
                }
            }

            AssembledCity assembledCity = new AssembledCity(10);
            assembledCity.AddTilePlacementIndex(
                new TileAndPlacementGroupIndex(
                    t,
                    cityEdgeGroupId
                )
            );
            foreach (AssembledCity ac in existingCities) {
                assembledCity.Merge(ac);
                AssembledCities.Remove(ac);
            }

            AssembledCities.Add(assembledCity);
        }
    }

    void IndexObelisksForTile(Tile t) {
        if (t.obelisk == null) return;

        AssembledObelisk ao = new AssembledObelisk(
            10,
            new TileAndPlacementGroupIndex(
                t,
                t.obelisk.GroupId
            )
        );
        AssembledObelisks.Add(ao);
    }

    void IndexRoadsForTile(Tile t) {
        List<RoadJoint> a = GetRoadJoints(t);
        MergeRoadJoints(a);
    }

    void MergeRoadJoints(List<RoadJoint> roadJoints) {
        foreach (RoadJoint rj in roadJoints) {
            if (rj == null) { continue; }
            AssembledRoad newAssembledRoad = new AssembledRoad(rj.placementIndex + 100);
            newAssembledRoad.AddTilePlacementIndex(
                    new TileAndPlacementGroupIndex(
                        rj.owningTile,
                        rj.placementIndex
                    )
                );
            if (rj.assembledRoads.Count > 0) {
                foreach(AssembledRoad ar in rj.assembledRoads) {
                    if (ar == null) { continue; }
                    newAssembledRoad.Merge(ar);
                    AssembledRoads.Remove(ar);
                }
            }
            AssembledRoads.Add(newAssembledRoad);
        }
    }

    List<RoadJoint> GetRoadJoints(Tile t) {
        List<RoadJoint> roadJoints = new List<RoadJoint>();
        foreach (Road r in t.Roads) {
            RoadJoint? sharedJoint = roadJoints.Find(rj => rj.placementIndex == r.RoadGroupId);
            if (sharedJoint == null) {
                sharedJoint = new RoadJoint(
                    t,
                    r.RoadGroupId,
                    new List<CardinalDirection>{ t.LocalToNormalizedDirection(r.localizedDirection) },
                    new List<AssembledRoad>()
                );
                roadJoints.Add(sharedJoint);
            } else {
                sharedJoint.NormalizedDirections.Add(t.LocalToNormalizedDirection(r.localizedDirection));
            }
        }

        foreach (RoadJoint rj in roadJoints) {
            foreach (CardinalDirection normalizedDir in rj.NormalizedDirections) {

                Tile Joiner = t.NormalizedSurvey.TileInDirection(normalizedDir);
                if (Joiner != null) {
                    CardinalDirection JoinerLocalizedDirection = GetOppositeDirection(Joiner.NormalizedToLocalDirection(normalizedDir));
                    Road AdjoiningRoad = Joiner.Roads.Find(r => r.localizedDirection == JoinerLocalizedDirection);

                    if (AdjoiningRoad == null) {
                        continue;
                    }

                    AssembledRoad AdjoiningAssembledRoad = AssembledRoads.Find(ar => ar.ContainsRoad(AdjoiningRoad));

                    if (rj.assembledRoads.Contains(AdjoiningAssembledRoad)) {
                        continue;
                    }

                    rj.assembledRoads.Add(AdjoiningAssembledRoad);
                }
            }
        }
        
        return roadJoints;
    }

    CardinalDirection GetOppositeDirection(CardinalDirection cd) {
        switch (cd) {
            case CardinalDirection.NORTH:
                return CardinalDirection.SOUTH;
            case CardinalDirection.EAST:
                return CardinalDirection.WEST;
            case CardinalDirection.SOUTH:
                return CardinalDirection.NORTH;
            case CardinalDirection.WEST:
                return CardinalDirection.EAST;
            default:
                return CardinalDirection.NORTH;
        }
    }
};
