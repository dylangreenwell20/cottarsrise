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

    //increments as it goes back? fix this
    //check to see if the room has already been visited - if it has, do not increment the room counter

    //store previous room direction from current room (e.g: if room goes from east to west, store that previous room is located to the east)
    //if next direction picked is the same as previous room location (e.g: direction picked is east and previous room is located to the east)
    //then proceed with room door generation, but do not increment the rooms created variable)
    //do not forget to store the previous room location again!!!

    private void Start()
    {
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

        int currentCell = startPos; //define start position

        //Vector2 currentCellPos = startPosition;

        Stack<int> path = new Stack<int>(); //create new stack

        int roomsNumber = 0; //number of rooms generated so far

        while(roomsNumber < 10) //while less than 10 rooms have been generated
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

                int newCell = neighbours[Random.Range(0, neighbours.Count)]; //pick random neighbour from neighbours list

                if(newCell > currentCell) //if new cell is going south or east
                {
                    if((newCell - 1) == currentCell) //check if room is east
                    {
                        board[currentCell].status[2] = true; //set east door of current cell to true
                        currentCell = newCell; //update current cell to new cell
                        board[currentCell].status[3] = true; //set west door of current cell to true

                        Debug.Log("room made east - " + roomsNumber);

                        roomsNumber++; //a room was created
                    }
                    else //else if the room is going south
                    {
                        board[currentCell].status[1] = true; //set south door of current cell to true
                        currentCell = newCell; //update current cell to new cell
                        board[currentCell].status[0] = true; //set north door  of current cell to true

                        Debug.Log("room made south - " + roomsNumber);

                        roomsNumber++; //a room was created
                    }

                    
                }
                else //the new cell is going north or west
                {
                    if ((newCell + 1) == currentCell) //check if room is west
                    {
                        board[currentCell].status[3] = true; //set west door of current cell to true
                        currentCell = newCell; //update current cell to new cell
                        board[currentCell].status[2] = true; //set east door of current cell to true

                        Debug.Log("room made west - " + roomsNumber);

                        roomsNumber++; //a room was created
                    }
                    else //else if the room is going south
                    {
                        board[currentCell].status[0] = true; //set north door of current cell to true
                        currentCell = newCell; //update current cell to new cell
                        board[currentCell].status[1] = true; //set south door of current cell to true

                        Debug.Log("room made north - " + roomsNumber);

                        roomsNumber++; //a room was created
                    }

                    //roomsNumber++; //a room was created
                }
            }
        }

        GenerateDungeon(); //generate the dungeon
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>(); //create neighbours list

        if(cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited) //check if there is a neighbour cell to the north which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x)); //add neighbour to list
        }

        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited) //check if there is a neighbour cell to the south which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x)); //add neighbour to list
        }

        if((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + size.x)].visited) //check if there is a neighbour cell to the east which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1)); //add neighbour to list
        }

        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell + size.x)].visited) //check if there is a neighbour cell to the west which isnt visited
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1)); //add neighbour to list
        }

        return neighbours;
    }
}
