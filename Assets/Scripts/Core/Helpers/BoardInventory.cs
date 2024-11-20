using System;
using System.Collections.Generic;
using System.Linq;

public class BoardInventory {
    public List<AssembledRoad> AssembledRoads = new List<AssembledRoad>();
    public List<AssembledObelisk> AssembledObelisks = new();
    public List<AssembledCity> AssembledCities = new();

    public void AddTileToInventory(Tile t) {
        IndexRoadsForTile(t);
        IndexObelisksForTile(t);
        IndexCitiesForTile(t);
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
                        .Find(ac => ac.tilePis.Any(tpi => tpi.placementIndex == oppoGroupId && tpi.tile == neighborTile));

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
