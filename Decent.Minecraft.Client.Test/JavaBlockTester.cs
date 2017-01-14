﻿using System;
using Decent.Minecraft.Client.Blocks;
using Decent.Minecraft.Client.Java;
using FluentAssertions;
using Xunit;

namespace Decent.Minecraft.Client.Test
{
    public class JavaBlockTester
    {
        public class For_a_stone_block
        {
            public class When_serializing
            {
                [Fact]
                public void It_should_return_a_JavaBlock_with_variants_as_bytes()
                {
                    var original = new Stone(Mineral.SmoothAndesite);

                    var javaBlock = JavaBlock.From(original);

                    javaBlock.TypeId.Should().Be(JavaBlockTypes.Id<Stone>());
                    javaBlock.Data.ToEnum<Mineral>().Should().Be(Mineral.SmoothAndesite);
                }
            }

            public class When_deserializing
            {
                [Theory]
                [InlineData((byte)Mineral.Stone, Mineral.Stone)]
                [InlineData((byte)Mineral.Andesite, Mineral.Andesite)]
                [InlineData(Mineral.SmoothGranite, Mineral.SmoothGranite)]
                [InlineData(0x20, (Mineral)0x20)]
                [InlineData(null, Mineral.Stone)]
                public void It_should_return_a_stone_block_with_the_variant_specified(byte serializedData, Mineral? expected)
                {
                    var actual = JavaBlock.Create(JavaBlockTypes.Id<Stone>(), serializedData) as Stone;
                    actual.Should().NotBeNull("deserializing a Stone block should return a Stone object");
                    actual.Mineral.Should().Be(expected, "the variant should be deserialized correctly");
                }
            }
        }

        public class For_an_end_stone
        {
            public class When_serializing
            {
                [Fact]
                public void It_should_return_a_JavaBlock_for_the_end_stone()
                {
                    var original = new EndStone();

                    var javaBlock = JavaBlock.From(original);

                    javaBlock.TypeId.Should().Be(JavaBlockTypes.Id<EndStone>());
                }
            }

            public class When_deserializing
            {
                [Fact]
                public void It_should_return_an_end_stone()
                {
                    var blockType = JavaBlockTypes.Id<EndStone>();

                    var deserialized = JavaBlock.Create(blockType, 0) as EndStone;

                    deserialized.Should().NotBeNull();
                }
            }
        }

        public class For_a_glowstone
        {
            public class When_serializing
            {
                [Fact]
                public void It_should_return_a_JavaBlock_for_the_glowstone()
                {
                    var original = new Glowstone();

                    var javaBlock = JavaBlock.From(original);

                    javaBlock.TypeId.Should().Be(JavaBlockTypes.Id<Glowstone>());
                }
            }

            public class When_deserializing
            {
                [Fact]
                public void It_should_return_a_glowstone()
                {
                    var blockType = JavaBlockTypes.Id<Glowstone>();

                    var deserialized = JavaBlock.Create(blockType, 0) as Glowstone;

                    deserialized.Should().NotBeNull();
                }
            }
        }

        public class For_a_liquid_block
        {
            public class When_serializing_and_deserializing
            {
                [Theory]
                [InlineData(Level.Source, true, false, 8, 0x0)]
                [InlineData(Level.Highest, true, true, 8, 0x9)]
                [InlineData(Level.Lowest, false, false, 9, 0x7)]
                [InlineData(Level.Mid, false, true, 9, 0xC)]
                public void It_should_round_trip_water(Level level, bool isFlowing, bool isFalling, int expectedId, byte expectedData)
                {
                    // Create the block from IBlock Properties
                    var original = new Water(level, isFlowing, isFalling);
                    javaBlockRoundTrip(level, isFlowing, isFalling, expectedId, expectedData, original);
                }

                [Theory]
                [InlineData(Level.Source, true, false, 10, 0x0)]
                [InlineData(Level.Highest, true, true, 10, 0x9)]
                [InlineData(Level.Lowest, false, false, 11, 0x7)]
                [InlineData(Level.Mid, false, true, 11, 0xC)]
                public void It_should_round_trip_lava(Level level, bool isFlowing, bool isFalling, int expectedId, byte expectedData)
                {
                    // Create the block from IBlock Properties
                    var original = new Lava(level, isFlowing, isFalling);
                    javaBlockRoundTrip(level, isFlowing, isFalling, expectedId, expectedData, original);
                }

                private void javaBlockRoundTrip(Level level, bool isFlowing, bool isFalling, int expectedId, byte expectedData, IBlock original)
                {
                    var javaBlock = JavaBlock.From(original);

                    javaBlock.TypeId.Should().Be(expectedId);
                    javaBlock.Data.Should().Be(expectedData);

                    // Create Block from id and data
                    var actual = JavaBlock.Create(expectedId, expectedData) as Liquid;

                    // Ensure the properties are equivalent coming from the other direction

                    actual.Level.Should().Be(level);
                    actual.IsFalling.Should().Be(isFalling);
                    actual.IsFlowing.Should().Be(isFlowing);
                }
            }
        }
        public class For_a_door_block
        {
            public class When_serializing_and_deserializing
            {
                [Theory]
                [InlineData(true, false, WoodSpecies.Oak, 64, 0x9)]
                [InlineData(false, false, WoodSpecies.DarkOak, 197, 0x8)]
                [InlineData(false, true, WoodSpecies.Spruce, 193, 0xA)]
                [InlineData(true, true, null, 71, 0xB)]
                public void It_should_round_trip_a_top(bool isHingeOnTheRight, bool isPowered, WoodSpecies? species, int expectedId, byte expectedData)
                {
                    if (species != null) // It's a wooden door
                    {
                        // Create the block from IBlock Properties
                        var original = new WoodenDoorTop(isHingeOnTheRight, isPowered, (WoodSpecies)species);
                        var javaBlock = JavaBlock.From(original);

                        javaBlock.TypeId.Should().Be(expectedId);
                        javaBlock.Data.Should().Be(expectedData);

                        // Create Block from id and data
                        var actual = JavaBlock.Create(expectedId, expectedData) as WoodenDoorTop;

                        // Ensure the properties are equivalent coming from the other direction.
                        actual.Species.Should().Be(species);
                        actual.IsPowered.Should().Be(isPowered);
                        actual.IsHingeOnTheRight.Should().Be(isHingeOnTheRight);

                    }
                    else // It's an iron door
                    {
                        // Create the block from IBlock Properties
                        var original = new IronDoorTop(isHingeOnTheRight, isPowered);
                        var javaBlock = JavaBlock.From(original);

                        javaBlock.TypeId.Should().Be(expectedId);
                        javaBlock.Data.Should().Be(expectedData);

                        // Create Block from id and data
                        var actual = JavaBlock.Create(expectedId, expectedData) as IronDoorTop;

                        // Ensure the properties are equivalent coming from the other direction.
                        actual.IsPowered.Should().Be(isPowered);
                        actual.IsHingeOnTheRight.Should().Be(isHingeOnTheRight);
                    }
                }

                [Theory]
                [InlineData(true, Direction.North, WoodSpecies.Birch, 194, 0x7)]
                [InlineData(false, Direction.South, WoodSpecies.Jungle, 195, 0x1)]
                [InlineData(true, Direction.East, WoodSpecies.Acacia, 196, 0x4)]
                [InlineData(false, Direction.West, null, 71, 0x2)]
                public void It_should_round_trip_bottom(bool isOpen, Direction facing, WoodSpecies? species, int expectedId, byte expectedData)
                {
                    if (species != null) // It's a wooden door
                    {
                        // Create the block from IBlock Properties
                        var original = new WoodenDoorBottom(isOpen, facing, (WoodSpecies)species);
                        var javaBlock = JavaBlock.From(original);

                        javaBlock.TypeId.Should().Be(expectedId);
                        javaBlock.Data.Should().Be(expectedData);

                        // Create Block from id and data
                        var actual = JavaBlock.Create(expectedId, expectedData) as WoodenDoorBottom;

                        // Ensure the properties are equivalent coming from the other direction.
                        actual.IsOpen.Should().Be(isOpen);
                        actual.Facing.Should().Be(facing);
                    }
                    else // It's an iron door
                    {
                        // Create the block from IBlock Properties
                        var original = new IronDoorBottom(isOpen, facing);
                        var javaBlock = JavaBlock.From(original);

                        javaBlock.TypeId.Should().Be(expectedId);
                        javaBlock.Data.Should().Be(expectedData);

                        // Create Block from id and data
                        var actual = JavaBlock.Create(expectedId, expectedData) as IronDoorBottom;

                        // Ensure the properties are equivalent coming from the other direction.
                        actual.IsOpen.Should().Be(isOpen);
                        actual.Facing.Should().Be(facing);

                    }
                }
            }
        }
    }

    public static class ByteExtensions
    {
        public static T ToEnum<T>(this byte input)
        {
            return (T)Enum.Parse(typeof(T), input.ToString());
        }
    }
}