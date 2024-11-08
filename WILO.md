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
- N1 N of B3
- K0 N of V0
- K0 N of W0
- U0 W of K0
- N0 W of V2
- N0 W of E0
- N2 E of G1
- K0 N of W0