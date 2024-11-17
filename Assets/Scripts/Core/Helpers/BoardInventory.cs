using System;
using System.Collections.Generic;
using System.Linq;

public class BoardInventory {
    public List<AssembledRoad> AssembledRoads = new List<AssembledRoad>();

    public void MergeRoadJoints(List<RoadJoint> roadJoints) {
        foreach (RoadJoint rj in roadJoints) {
            if (rj == null) { continue; }
            AssembledRoad newAssembledRoad = new AssembledRoad(rj.placementIndex + 100);
            newAssembledRoad.AddTilePlacementIndex(
                    new TileAndPlacementIndex(
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

    public List<RoadJoint> GetRoadJoints(Tile t) {
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
