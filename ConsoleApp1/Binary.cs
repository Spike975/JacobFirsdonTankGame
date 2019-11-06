using System;
using System.Collections.Generic;
using System.Text;

namespace NewRaylibGame
{
    class Binary
    {
        const byte CHAINSAW = 0x01;
        const byte PISTOL = 0x01 << 1;
        const byte SHOTGUN = 0x01 << 2;
        const byte SUPER_SHOTGUN = 0x01 << 3;
        const byte CHAINGUN = 0x01 << 4;
        const byte ROCKET_LAUNCHER = 0x01 << 5;
        const byte PLASMA_GUN = 0x01 << 6;
        const byte BFG9000 = 0x01 << 7;        public static readonly string[] weapons = {"Fists", "Chainsaw", "Pistol", "Shotgun", "Super Shotgun", "Chaingun", "Rocket Launcher", "Plasma Gun", "BFG 9000"};        byte inventory;        public Binary()
        {
            inventory = 0x43;
        }
        public Binary(byte weapons)
        {
            inventory = weapons;
        }
        public void PrintInventory(byte inventory) {
            Console.Write($"{weapons[0]} | ");
            for (int i = 1; i < weapons.Length; i++)
            {
                int mask = 0x01 << i - 1;
                if ((inventory & mask) == mask)
                {
                    Console.Write($"{weapons[i]} | ");
                }
            }
            Console.Write("\n");
        }
        public static void AddToInventory(ref byte inventory, byte weapon)
        {
            inventory |= weapon;
        }

    }
}
