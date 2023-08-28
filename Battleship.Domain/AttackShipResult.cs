namespace Battleship.Domain;

public record AttackShipResult(bool Hit, bool SunkShip, string Message);
public record AddShipResult(bool Added, string Message);