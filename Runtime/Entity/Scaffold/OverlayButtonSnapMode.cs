namespace Playtolia.Entity.Scaffold
{
    /// <summary>How the Playtolia floating button settles when the player releases a drag.</summary>
    /// <remarks>
    /// The three edge modes are named after the edges they allow, not the axis the button snaps
    /// along, because those two readings are opposites.
    /// </remarks>
    public enum OverlayButtonSnapMode
    {
        /// <summary>Snap to the nearest of the four screen corners.</summary>
        Corner,

        /// <summary>Keep the released height and hug the left or right edge. The default.</summary>
        EdgeLeftRight,

        /// <summary>Stay exactly where released. Pushing into an eligible edge still minimizes.</summary>
        Free,

        // Appended rather than grouped with EdgeLeftRight so the values above keep the numbers
        // they were serialized with before top and bottom docking existed.

        /// <summary>Keep the released horizontal position and hug the top or bottom edge.</summary>
        EdgeTopBottom,

        /// <summary>Hug whichever of the four edges is nearest, keeping the position along it.</summary>
        EdgeAny,
    }
}
