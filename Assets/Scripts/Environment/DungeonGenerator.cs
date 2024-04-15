using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false; //has cell been visited by depth first algorithm
        public bool[] status = new bool[4]; //status of each entrance of the cell
        public bool[] isDoorInvisible = new bool[4]; //should the door be invisible
        public bool isStartingRoom = false; //is the room a starting room - false by default
        public bool isBossRoom = false; //is the room a boss (final) room - false by default
        public int roomType = 5; //which room prefab to use
    }

    public Vector2 size; //size of dungeon board (x and y cells)

    public int startPos; //start position for generation

    public GameObject roomPrefab; //reference to dungeon room prefab - currently a temporary room for testing
    public GameObject startRoomPrefab; //starting room for dungeon
    public GameObject combatRoom1Prefab; //combat room 1 for dungeon
    public GameObject combatRoom2Prefab; //combat room 2 for dungeon
    public GameObject combatRoom3Prefab; //combat room 3 for dungeon
    public GameObject lootRoom1Prefab; //loot room 1 for dungeon
    public GameObject teleporterPrefab; //room to teleport to boss

    public Vector2 offset; //offset for rooms to be generated - make multiple for 1x1 offset, 2x2 offset etc

    List<Cell> board; //create board for cells

    public int genOrder = 0; //for testing

    private bool directionCheck; //loop to check if room is generating to a previously-made room

    public int previousGenDirection; //previous direction a room was generated in (0 = north, 1 = east, 2 = south, 3 = west)

    public bool sameDirectionTwice; //if 2 rooms were generated in the same direction - used because unity.random is (for some reason) not random all the time
    //and using this bool i can prevent every other dungeon from being a straight line north (yes this happens too much it should be impossible)

    public bool startingRoom; //if starting room is being generated

    public List<int> roomTypeList = new List<int> { 0, 1, 2, 3 }; //for what room should be generated

    private int previousRoomNumber = 5; //previously generated room number - to stop the same room being generated twice

    public List<GameObject> createdRooms = new List<GameObject>(); //create gameobject list to store created rooms

    public PlayerMovement pm; //reference to player movement

    public NormalOrDeadUI uiStatus; //change ui status

    public PerkHolder perkHolder; //to apply perk to player

    public GeneratePerks genPerks; //to update what perk was selected

    public PlayerHealth playerHealth;
    public PlayerMana playerMana;

    public AbilityHolder abilityHolder;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond); //random seed for truly random rooms

        MazeGenerator(); //generate the maze
        GetComponent<NavMeshSurface>().BuildNavMesh(); //build nav mesh after dungeon is generated
        SpawnEnemies(); //spawn room enemies

        AudioManager.Instance.musicSource.Stop(); //stop current music
        AudioManager.Instance.PlayMusic("GameMusic"); //play game music

        perkHolder.ApplyPerk(); //apply chosen perk
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
                    if(currentCell.isStartingRoom)
                    {
                        var newRoom = Instantiate(startRoomPrefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                        newRoom.UpdateRoom(currentCell.status, currentCell.isDoorInvisible); //update room on the board

                        newRoom.MovePlayer(); //get player spawn location

                        //pm.SpawnPlayer(); //move player to spawn location

                        newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing
                        genOrder++; //for testing
                    }
                    else if (currentCell.isBossRoom)
                    {
                        var newRoom = Instantiate(teleporterPrefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                        newRoom.UpdateRoom(currentCell.status, currentCell.isDoorInvisible); //update room on the board

                        newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing
                        genOrder++; //for testing
                    }
                    else
                    {
                        if (currentCell.roomType == 0)
                        {
                            var newRoom = Instantiate(combatRoom1Prefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                            newRoom.UpdateRoom(currentCell.status, currentCell.isDoorInvisible); //update room on the board

                            previousRoomNumber = 0;

                            newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing

                            createdRooms.Add(newRoom.gameObject);

                            genOrder++; //for testing
                        }
                        else if (currentCell.roomType == 1)
                        {
                            var newRoom = Instantiate(combatRoom2Prefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                            newRoom.UpdateRoom(currentCell.status, currentCell.isDoorInvisible); //update room on the board

                            previousRoomNumber = 1;

                            newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing

                            createdRooms.Add(newRoom.gameObject);

                            genOrder++; //for testing
                        }
                        else if (currentCell.roomType == 2)
                        {
                            var newRoom = Instantiate(combatRoom3Prefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                            newRoom.UpdateRoom(currentCell.status, currentCell.isDoorInvisible); //update room on the board

                            previousRoomNumber = 2;

                            newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing

                            createdRooms.Add(newRoom.gameObject);

                            genOrder++; //for testing
                        }
                        else if (currentCell.roomType == 3)
                        {
                            var newRoom = Instantiate(lootRoom1Prefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                            newRoom.UpdateRoom(currentCell.status, currentCell.isDoorInvisible); //update room on the board

                            previousRoomNumber = 3;

                            newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing

                            createdRooms.Add(newRoom.gameObject);

                            genOrder++; //for testing
                        }
                        else //else if there is a bug - just create empty room
                        {
                            var newRoom = Instantiate(roomPrefab, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomScript>(); //instantiate new room
                            newRoom.UpdateRoom(currentCell.status, currentCell.isDoorInvisible); //update room on the board

                            previousRoomNumber = 5;

                            newRoom.name += " " + i + "-" + j + " GEN " + genOrder; //name the room with its position - and gen for testing
                            genOrder++; //for testing
                        }
                    }
                }
            }
        }
    }

    void SpawnEnemies()
    {
        for(int i = 0; i < createdRooms.Count; i++)
        {
            createdRooms[i].gameObject.GetComponent<RoomScript>().SpawnEnemies(); //spawn enemies
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

        previousGenDirection = 5; //set to 5 as this is not a valid position so it will not effect previous direction checks later on

        int currentCell = startPos; //define start position

        startingRoom = true; //start room must be generated

        Stack<int> path = new Stack<int>(); //create new stack

        int roomsNumber = 0; //number of rooms generated so far

        sameDirectionTwice = false; //set to false for same direction checks

        while (roomsNumber < 10) //while less than 10 rooms have been generated
        {
            board[currentCell].visited = true; //current cell has been visited

            if(startingRoom) //if start room hasnt been made yet
            {
                board[currentCell].isStartingRoom = true; //set current room to start room
                startingRoom = false; //set to false to not make any more start rooms
            }

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
                    //Debug.Log(currentCell + " popped!");
                }
            }
            else
            {
                path.Push(currentCell); //push currentCell to stack

                if (roomsNumber == 9) //if the final room is being generated
                {
                    board[currentCell].isBossRoom = true; //set cell to boss room
                }

                if(roomsNumber != 0 && roomsNumber != 9)
                {
                    List<int> roomList = new List<int>(roomTypeList); //clone list for temp storage

                    int prefabType = Random.Range(0, roomList.Count); //what room prefab to generate

                    //Debug.Log(prefabType);

                    if (prefabType == previousRoomNumber) //if room type is the same as the previously generated room
                    {
                        //Debug.Log("two of the same room type in a row - pick a new one");

                        roomList.RemoveAt(prefabType); //remove current room type from the cloned list
                        prefabType = Random.Range(0, roomList.Count); //pick another room

                        //Debug.Log(prefabType);

                        //Debug.Log("cleared");
                    }

                    board[currentCell].roomType = prefabType; //set the prefab type of the room

                    previousRoomNumber = prefabType; //set previous number to avoid two of the same room in a row
                }

                directionCheck = false; //set to false for room direction check loop

                while (!directionCheck) //while previous direction has not been checked yet (room cannot generate to the direction it came from)
                {
                    int randomIndex = Random.Range(0, neighbours.Count); //pick a random index from the list of neighbours

                    int newCell = neighbours[randomIndex]; //pick random neighbour from neighbours list

                    //Debug.Log("count: " + neighbours.Count);
                    //Debug.Log("index: " + randomIndex);
                    //Debug.Log("cell: " + newCell);

                    if (board[newCell].visited) //if cell already visited
                    {
                        //Debug.Log("visited already"); //debug and make another neighbour be picked
                        neighbours.RemoveAt(randomIndex); //remove neighbour from the list
                    }
                    else //else if neighbour hasnt been visited
                    {
                        if (newCell > currentCell) //if new cell is going south or east
                        {
                            if ((newCell - 1) == currentCell) //check if room is east
                            {
                                //Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 1 && sameDirectionTwice == true) //if rooms have been created in the same direction twice before and is going in the same direction
                                {
                                    //Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    if(roomsNumber != 9) //if it is not the final room
                                    {
                                        board[currentCell].status[2] = true; //set east door of current cell to true
                                        currentCell = newCell; //update current cell to new cell
                                        board[currentCell].status[3] = true; //set west door of current cell to true
                                        board[currentCell].isDoorInvisible[3] = true; //make the door invisible to stop overlapping doors
                                    }

                                    //Debug.Log("room made east - " + roomsNumber);

                                    if(previousGenDirection == 1) //if previous room was created to the east
                                    {
                                        sameDirectionTwice = true; //two rooms have been generated in the same direction
                                        //Debug.Log("east twice!");
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
                                //Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 2 && sameDirectionTwice == true) //if previous direction was south and two have been made in that direction
                                {
                                    //Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    if(roomsNumber != 9) //if it is not the final room
                                    {
                                        board[currentCell].status[1] = true; //set south door of current cell to true
                                        currentCell = newCell; //update current cell to new cell
                                        board[currentCell].status[0] = true; //set north door  of current cell to true
                                        board[currentCell].isDoorInvisible[0] = true; //make the door invisible to stop overlapping doors
                                    }
                                    
                                    //Debug.Log("room made south - " + roomsNumber);

                                    if(previousGenDirection == 2) //if previous room was created to the south
                                    {
                                        sameDirectionTwice = true; //two rooms have been generated in the same direction
                                        //Debug.Log("south twice!");
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
                                //Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 3 && sameDirectionTwice == true)
                                {
                                    //Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    if (roomsNumber != 9) //if it is not the final room
                                    {
                                        board[currentCell].status[3] = true; //set west door of current cell to true
                                        currentCell = newCell; //update current cell to new cell
                                        board[currentCell].status[2] = true; //set east door of current cell to true
                                        board[currentCell].isDoorInvisible[2] = true; //make the door invisible to stop overlapping doors
                                    }

                                    //Debug.Log("room made west - " + roomsNumber);

                                    if(previousGenDirection == 3)
                                    {
                                        sameDirectionTwice = true;
                                        //Debug.Log("west twice!");
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
                                //Debug.Log("pre: " + previousGenDirection + ", " + sameDirectionTwice);

                                if (previousGenDirection == 0 && sameDirectionTwice == true)
                                {
                                    //Debug.Log("3rd time going in the same direction - change the path");
                                    neighbours.RemoveAt(randomIndex); //remove that neighbour from the list

                                    sameDirectionTwice = false; //set back to false
                                }
                                else
                                {
                                    if (roomsNumber != 9) //if it is not the final room
                                    {
                                        board[currentCell].status[0] = true; //set north door of current cell to true
                                        currentCell = newCell; //update current cell to new cell
                                        board[currentCell].status[1] = true; //set south door of current cell to true
                                        board[currentCell].isDoorInvisible[1] = true; //make the door invisible to stop overlapping doors
                                    }

                                    //Debug.Log("room made north - " + roomsNumber);

                                    if(previousGenDirection == 0)
                                    {
                                        sameDirectionTwice = true;
                                        //Debug.Log("north twice!");
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

    public void RestartDungeon() //for creating a new dungeon when the player chooses to continue (so they can keep their equipment instead of reloading the scene)
    {
        while(transform.childCount > 0) //while object has more than 1 child
        {
            DestroyImmediate(transform.GetChild(0).gameObject); //destroy immediate child
        }

        for (int i = 0; i < size.x; i++) //for each cell on the x axis
        {
            for (int j = 0; j < size.y; j++) //for each cell on the y axis
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)]; //get current cell

                if (currentCell.visited) //if cell is visited then reset its variables
                {
                    currentCell.visited = false;
                    currentCell.isStartingRoom = false;
                    currentCell.isBossRoom = false;
                    currentCell.status = new bool[4];
                    currentCell.isDoorInvisible = new bool[4];
                    currentCell.roomType = 5;
                }
            }
        }

        pm.bossButtonPressed = false; //boss button can be pressed again

        createdRooms = new List<GameObject>(); //reset created rooms list

        MazeGenerator(); //generate the maze
        GetComponent<NavMeshSurface>().BuildNavMesh(); //build nav mesh after dungeon is generated
        SpawnEnemies(); //spawn room enemies

        AudioManager.Instance.musicSource.Stop(); //stop current music
        AudioManager.Instance.sfxSource.Stop(); //stop current sfx
        AudioManager.Instance.PlayMusic("GameMusic"); //play game music

        pm.SpawnPlayer(); //move player to dungeon spawn

        genPerks.UpdateStaticPerk(); //update static perk to allow for it to be applied

        perkHolder.ApplyPerk(); //apply chosen perk

        Inventory.instance.arrowCount = 50; //max arrows

        playerHealth.HealPlayer(9999); //heal player over max health
        playerMana.HealMana(9999); //heal player over max mana

        uiStatus.NormalUI(); //change to normal ui
    }
}
