using System.Collections.Generic;
using UnityEngine;

public static class CellReservation
{
    // HashSet para guardar as células reservadas
    private static HashSet<Vector2> reservedCells = new HashSet<Vector2>();

    // Reserva uma célula, retorna true se foi reservada com sucesso
    public static bool ReserveCell(Vector2 cell)
    {
        if (!reservedCells.Contains(cell))
        {
            reservedCells.Add(cell);
            return true;
        }
        return false;
    }

    // Verifica se a célula está reservada
    public static bool IsCellReserved(Vector2 cell)
    {
        return reservedCells.Contains(cell);
    }

    // Libera uma célula
    public static void ReleaseCell(Vector2 cell)
    {
        if (reservedCells.Contains(cell))
        {
            reservedCells.Remove(cell);
        }
    }
}
