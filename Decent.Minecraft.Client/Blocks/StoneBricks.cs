﻿namespace Decent.Minecraft.Client.Blocks
{
    /// <summary>
    /// <a href="http://minecraft.gamepedia.com/Stone_Bricks">Gamepedia link</a>.
    /// </summary>
    public class StoneBricks : IBlock
    {
        public StoneBricks() : this(StoneQuality.Normal) { }

        protected StoneBricks(StoneQuality quality)
        {
            Quality = quality;
        }

        public StoneQuality Quality { get; }

        public enum StoneQuality : byte
        {
            Normal = 0,
            Mossy,
            Cracked,
            Chiseled
        }
    }

    /// <summary>
    /// <a href="http://minecraft.gamepedia.com/Stone_Bricks">Gamepedia link</a>.
    /// </summary>
    public class MossyStoneBricks : StoneBricks
    {
        public MossyStoneBricks() : base(StoneQuality.Mossy) { }
    }

    /// <summary>
    /// <a href="http://minecraft.gamepedia.com/Stone_Bricks">Gamepedia link</a>.
    /// </summary>
    public class CrackedStoneBricks : StoneBricks
    {
        public CrackedStoneBricks() : base(StoneQuality.Cracked) { }
    }

    /// <summary>
    /// <a href="http://minecraft.gamepedia.com/Stone_Bricks">Gamepedia link</a>.
    /// </summary>
    public class ChiseledStoneBricks : StoneBricks
    {
        public ChiseledStoneBricks() : base(StoneQuality.Chiseled) { }
    }
}
