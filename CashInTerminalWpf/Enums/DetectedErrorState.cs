namespace CashInTerminal.Enums
{
    public enum DetectedErrorState
    {
        Unknown = 0x0,
        Other = 0x1,
        NoError = 0x2,
        LowPaper = 0x3,
        NoPaper = 0x4,
        LowToner = 0x5,
        NoToner = 0x6,
        DoorOpen = 0x7,
        Jammed = 0x8,
        Offline = 0x9,
        ServiceRequested = 0x10,
        OutputBinFull = 0x11
    }
}
