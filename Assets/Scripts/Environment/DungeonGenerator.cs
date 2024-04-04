using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false; //has cell been visited by depth first algorithm
        public bool[] status = new bool[4]; //status of each entrance of the cell
    }

    public Vector2 size; //size of dungeon board (x and y cells)

    public int startPos; //start position for generation

    public GameObject roomPrefab; //reference to dungeon room prefab - add more in future

    public Vector2 offset; //offset for rooms to be generated - make multiple for 1x1 offset, 2x2 offset etc

    List<Cell> board; //create board for cells

    public int genOrder = 0; //for testing

    private bool directionCheck; //loop to check if room is generating to a previously-made room

    public int previousGenDirection; //previous direction a room was generated in (0 = north, 1 = east, 2 = south, 3 = west)

    public bool sameDirectionTwice; //if 2 rooms were generated in the same direction - used because unity.random is (for some reason) not random all the time
    //and using this bool i can prevent every other dungeon from being a straight line north (yes this happens too much it should be impossible)

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond); //random seed for truly random rooms

        MazeGenerator(); //generate the maze
    }

    void GenerateDungeon()
    {
        for(int i = 0; i < size.x; i++) //for each cell on the x axis
        {
            for (int j = 0; j < size.y; j++) //for each cell on the y axis
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)]; //get current cell

                if (currentCell.visited) //if cell was visited (essentially if it was generated - this stops a blank room being generated for each cell of the grid)
                {
                    var newRoom = Instantiate(roomPrefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                    newRoom.UpdateRoom(currentCell.status); //update room on the board

                    newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing
                    genOrder++; //for testing
                }
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>(); //create new dungeon board

        for(int i = 0; i < size.x; i++) //for x value of dungeon board size
        {
            for(int j = 0; j < size.y; j++) //for y value of dungeon board size
            {
                board.Add(new Cell()); //add new cell to board
            }
        }

        previousGenDirection = 5; // set to 5 as this is not a valid position so it will not effect previous direction checks later on

        int currentCell = startPos; //define start position

        Stack<int> path = new Stack<int>(); //create new stack

        int roomsNumber = 0; //number of rooms generated so far

        sameDirectionTwice = false; //set to false for same direction checks

        while (roomsNumber < 10) //while less than 10 rooms have been generated
        {
            board[currentCell].visited = true; //current cell has been visited

            List<int> neighbours = CheckNeighbours(currentCell); //get list of neighbours

            if(neighbours.Count == 0) //if there are no neighbours
            {
                if(path.Count == 0) //if no items in path stack
                {
                    break; //stop while loop
                }
                else //else if items are in stack
                {
                    currentCell = path.Pop(); //pop current value so currentCell becomes the previous cell
                    Debug.Log(currentCell + " popped!");
                }
            }
            else
            {
                path.Push(currentCell); //push currentCell to stack

                directionCheck = false; //set to false for room direction check loop

                while (!directionCheck) //while previous direction has not been checked yet (room cannot generate to the direction it came from)
                {
                    int randomIndex = Random.Range(0, neighbours.Count); //pick a random index from the list of neighbours

                    int newCell = neighbours[randomIndex]; //pick random neighbour from neighbours list

                    for(int i = 0; i < neighbours.Count; i++)
                    {
                        Debug.Log(neighbours[i]);
                    }

                    Debug.Log("count: " + neighbours.Count);
                    Debug.Log("index: " + randomIndex);
                    Debug.Log("cell: " + newCell);

                    if (board[newCell].visited) //if cell already visited
                    {
                        Debug.Log("visited already"); //debug and make another neighbour be picked
                        neighbours.RemoveAt(randomIndex); //remove neighbour from the list
                    }
                    else //else if neighbour hasnt been visited
                    {
                        if (newCell > currentCell) //if new cell is going south or east
                        {
                            if ((newCell - 1) == currentCell) //check if room is east
                            {
                                Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 1 && sameDirectionTwice == true) //if rooms have been created in the same direction twice before and is going in the same direction
                                {
                                    Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    board[currentCell].status[2] = true; //set east door of current cell to true
                                    currentCell = newCell; //update current cell to new cell
                                    board[currentCell].status[3] = true; //set west door of current cell to true

                                    Debug.Log("room made east - " + roomsNumber);

                                    if(previousGenDirection == 1) //if previous room was created to the east
                                    {
                                        sameDirectionTwice = true; //two rooms have been generated in the same direction
                                        Debug.Log("east twice!");
                                    }
                                    else
                                    {
                                        sameDirectionTwice = false;
                                    }

                                    previousGenDirection = 1; //future rooms can tell the last direction was east

                                    //Debug.Log("current: " + previousGenDirection);

                                    roomsNumber++; //a room was created

                                    directionCheck = true; //end direction check loop
                                }
                            }
                            else //else if the room is going south
                            {
                                Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 2 && sameDirectionTwice == true) //if previous direction was south and two have been made in that direction
                                {
                                    Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    board[currentCell].status[1] = true; //set south door of current cell to true
                                    currentCell = newCell; //update current cell to new cell
                                    board[currentCell].status[0] = true; //set north door  of current cell to true

                                    Debug.Log("room made south - " + roomsNumber);

                                    if(previousGenDirection == 2) //if previous room was created to the south
                                    {
                                        sameDirectionTwice = true; //two rooms have been generated in the same direction
                                        Debug.Log("south twice!");
                                    }
                                    else
                                    {
                                        sameDirectionTwice = false;
                                    }

                                    previousGenDirection = 2; //future rooms can tell the last direction was south

                                    //Debug.Log("current: " + previousGenDirection);

                                    roomsNumber++; //a room was created

                                    directionCheck = true; //end direction check loop
                                }
                            }
                        }
                        else //the new cell is going north or west
                        {
                            if ((newCell + 1) == currentCell) //check if room is west
                            {
                                Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 3 && sameDirectionTwice == true)
                                {
                                    Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    board[currentCell].status[3] = true; //set west door of current cell to true
                                    currentCell = newCell; //update current cell to new cell
                                    board[currentCell].status[2] = true; //set east door of current cell to true

                                    Debug.Log("room made west - " + roomsNumber);

                                    if(previousGenDirection == 3)
                                    {
                                        sameDirectionTwice = true;
                                        Debug.Log("west twice!");
                                    }
                                    else
                                    {
                                        sameDirectionTwice = false;
                                    }

                                    previousGenDirection = 3; //future rooms can tell the last direction was west

                                    //Debug.Log("current: " + previousGenDirection);

                                    roomsNumber++; //a room was created

                                    directionCheck = true; //end direction check loop
                                }
                            }
                            else //else if the room is going north
                            {
                                Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 0 && sameDirectionTwice == true)
                                {
                                    Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    board[currentCell].status[0] = true; //set north door of current cell to true
                                    currentCell = newCell; //update current cell to new cell
                                    board[currentCell].status[1] = true; //set south door of current cell to true

                                    Debug.Log("room made north - " + roomsNumber);

                                    if(previousGenDirection == 0)
                                    {
                                        sameDirectionTwice = true;
                                        Debug.Log("north twice!");
                                    }
                                    else
                                    {
                                        sameDirectionTwice = false;
                                    }

                                    previousGenDirection = 0; //future rooms can tell the last direction was north

                                    //Debug.Log("current: " + previousGenDirection);

                                    roomsNumber++; //a room was created

                                    directionCheck = true; //end direction check loop
                                }
                            }
                        }
                    }
                }
            }
        }

        GenerateDungeon(); //generate the dungeon
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>(); //create neighbours list

        if(cell - 15 >= 0 && !board[Mathf.FloorToInt(cell - 15)].visited) //check if there is a neighbour cell to the north which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell - 15)); //add neighbour to list
        }

        if((cell + 1) % 15 != 0 && !board[Mathf.FloorToInt(cell + 1)].visited) //check if there is a neighbour cell to the east which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1)); //add neighbour to list
        }

        if (cell + 15 < board.Count && !board[Mathf.FloorToInt(cell + 15)].visited) //check if there is a neighbour cell to the south which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell + 15)); //add neighbour to list
        }

        if ((cell - 1) % 15 != 0 && !board[Mathf.FloorToInt(cell - 1)].visited) //check if there is a neighbour cell to the west which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1)); //add neighbour to list
        }

        return neighbours;
    }
}
