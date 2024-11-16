### 11/6/24
Current idea is building toward the starting setup: 
- cycle tiles on the right bar
- allow clicking grid nodes to place
- grid nodes green if placeable
- right bar tile is rotatable
- build city until you're out of tiles

WILO: I have some debug logs to clean up. testing has saved the day on some tricky bits. Everything is getting close but now need to write the actual tile with face loaded and adjusted for the rotation. then need to hook up the NEXT button on the right pane.

NEXT STEPS: either take turns with a CPU -OR- click-swipe to pan the map area inside bounding box


### 11/7/2024 (early)
Grid drops are working. Now need to collect up broken combos. example: `N1 N of B3`  (N rotated once, B rotated thrice) 


SHOULD BE FALSE
X N1 N of B3
X K0 N of V0
X K0 N of W0
X U0 W of K0
X N0 W of V2
X N0 W of E0
X N2 E of G1


### 11/10/24
What functionality needs to be put together for this to be a shippable MVP?
- Title Screen
- Tutorial/Walkthrough
- End of Game Sequence
- Scoring
- Placing meeples
- backgrounds
- custom art tiles
- GUI for main scene


### 11/16/24
Can select from eligible terraformer locations and place one
- Need a player inventory
    - count of TFs
    - points
    - secret missions
- Find if thye have a terraformer before offering that step
- Need a scoring algorithm that crawls the tiles
    - Need a way for a Tile to query out every feature of .CanSitNextTo checks to see if the feature is occupied

- Maybe when you place a card, it adds it to any connected features in a formal registry or registers a new one
    - Also register to neighbors? then they could count vicinity..
- need to add real game pieces to the board/tile
- need Obelisk to be a location of interest like streets. no effect on can-i-place

