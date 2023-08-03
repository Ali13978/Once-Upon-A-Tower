using System;

[Flags]
public enum TKSwipeDirection
{
	Left = 0x1,
	Right = 0x2,
	Up = 0x4,
	Down = 0x8,
	UpLeft = 0x10,
	DownLeft = 0x20,
	UpRight = 0x40,
	DownRight = 0x80,
	Horizontal = 0x3,
	Vertical = 0xC,
	Cardinal = 0xF,
	DiagonalUp = 0x50,
	DiagonalDown = 0xA0,
	DiagonalLeft = 0x30,
	DiagonalRight = 0xC0,
	Diagonal = 0xF0,
	RightSide = 0xC2,
	LeftSide = 0x31,
	TopSide = 0x54,
	BottomSide = 0xA8,
	All = 0xFF
}
