Player Input:

Player1:
A,D,S,W for movement.
Q - To pick up an Item.
E - To place/drop an Item.

Player2:

J,L,K,I for movement.
U - To pick up an Item
O - To place/drop an Item.

Sample Case: Player 1

Use the mentioned input to control the player and navigate to the Vegetable stock on either side (blue and red table). Use Q to pick up the respective item by navigating near to the item. Use E to drop it. You can carry one/two items at a time and place on chopboard. Max Items allowed are 3 on chopboard. Timer is set for each of the item to be chopped. Once an item placed, the timer kicks off. Player cannot move till the item is chopped. Once all the items are chopped, use Q to pick the Salad and navigate to the corresponding customer. If the items are matched(irrespective of the order), customer will show a green light. Else a red light.


Tasks ignored/not implemented yet:

1) There is no step of placing the salad on the plate seperately, once the vegetables are chopped and the player presses the pick button, a plate with corresponding vegetables are instantiated as salad.
2) High scores are not added as of now.
Implementation: Just a single integer list that updates the list of highscores upon ending of each game. When any one of the player's score is maximum than the minimum score of the highscore list, the current score enters the list, popping out the previous minimum.


Project Structure:

Contains compile-time instances of 2 players and 5 customers. This can also be extended to any number of players and customers by extending the CustomerController script. Contains different GameObjects for UI. Hierarchy is not structured as of now.

Each player is controlled by PlayerControl script.

Project is developed in 4 steps.

1) Basic UI design and modularizing the different components required( Player, Customer, UI handler and GameController).
2) Programming the player. (Involves picking and dropping the vegetables at different triggers - Veg Stock, ChopBoard, Trashcan and Customer)
3) Customer Controller. (Creating random order on a defined time interval (can be extended to create a random timer also))
4) All the UI update based on different states of Players and Customers.


Due to other commitments, I couldn't start the project early and commit at different steps for a detailed understanding. Kindly reach out to me incase of project related queries.

