using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Models;

public record struct ItemStack(int Type, int Stack, short Prefix);