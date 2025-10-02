using System;

namespace TallyBoard.App.Models;

public class TallyState
{
    public int Count { get; set; }
    public string? Note { get; set; }
    public DateTime SavedAtUtc { get; set; } = DateTime.UtcNow;
}
