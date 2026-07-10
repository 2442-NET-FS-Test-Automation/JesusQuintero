using Warehouse.Data.Entities;

namespace Warehouse.Data.Records;

public record MaterialMovementDto(int idMaterial, int quantity);
public record EntryMaterialDto(List<MaterialMovementDto> materials,int userId, int toBin);
public record MoveMaterialDto(List<MaterialMovementDto> materials,int userId, int fromBin, int toBin);
public record ShipMaterialDto(List<MaterialMovementDto> materials,int userId, int fromBin);
public record GenericMaterialMovementDto(MovementTypes Movement, List<MaterialMovementDto> materials,int userId, int fromBin = 0, int toBin = 0);
public record BurstResult(int compEntries, int compMovements, int compShipments, int failMovements, int failShipments);