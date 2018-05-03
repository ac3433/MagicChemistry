# Magic Chemistry
In Magic Chemistry, a player will be a member of a magical lab group that creates various potions. The player is expected to help their group by combining elements into a cauldron. Various equipment such as beakers, tubes, and filters to formulate the correct potion. The final potion value will be provided for the player. The potion will change colors as the player makes changes to the formula. This will allow the player to have a non-alphanumeric visual cue to help them determine what the steps are necessary next. If the player is struggling with a formula, a lab partner (NPC) may come over and give them a hint after a random period of time. After a few moments, the player may be allowed to “Ask for Help” where a lab partner (NPC) will walk the player through the steps to solve their potion.

When the player begins, they may only have access to a few operations and a limited set of outputs. However, as they prove their mastery, more operations will open up and allow for more complex potions. The potions may have operations that are locked in place that the player cannot avoid using which may also preclude the use of similar operations. Other potential advancements would be to use “faulty” equipment that have some variable associated with them, such as a leaky pipe.

The game will feature addition, subtraction, multiplication, division, and modulo division. Since the game is meant for players to combine various numbers and operators, the game would utilize division operators as both a rounded-down form and using the remainders. This is to reduce the number of computations necessary to resolve a floating point number, but to also show that remainders are useful. 

# Instructions:

The objective of the game is to produce a desired output by linking numbered tubes on the grid with operations.
At the top left is the starting input. At the bottom right is the desired output.
At the top right is the amount of time left before liquid starts flowing from the starting input. At the bottom left is the current value traveling through the tubes.
On the left-hand side is the template panel. It contains templates of regular, empty tubes and operation tubes. To put a tube on the grid, simply left click and drag from a template tube onto the grid. Left click a tube on the grid to rotate it, and right click it to delete the tube.
Numbered tubes must have an operation tube in between them to perform operations. For example, to add two numbered tubes '1' and '2', a '+' tube must be on the path connecting '1' and '2'.
Once the current value is equal to the desired output, simply route the tubes to the desired output without any operations.

## Unity Version 2017.3.1
