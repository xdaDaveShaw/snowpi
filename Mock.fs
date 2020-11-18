module Mock

open Colorful
open System.Drawing

let run (c : Color) = 
    Console.WriteLineFormatted("""
    
    ###############
     #############
      ###########
       #########
   #################
     /           \
    /  [{0}]   [{0}]  \
   |               |
    \     [{0}]     /
     \           /
     /           \
    /     [{0}]     \
   / [{0}]       [{0}] \
  /       [{0}]       \
 |  [{0}]         [{0}]  |
  \       [{0}]       /
   \[{0}]         [{0}]/
    \_____________/

    """, c, Color.White, "X")