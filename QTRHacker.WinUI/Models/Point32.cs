using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Models;

[StructLayout(LayoutKind.Sequential)]
public readonly record struct Point32(int X, int Y)
{
}