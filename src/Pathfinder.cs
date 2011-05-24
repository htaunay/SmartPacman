using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SmartPacMan
{
	/// <summary>
	/// 2D grid, A*, Manhattan distance pathfinding class.
	/// The Pathfinder needs a map informing its size and valid/invalid positions,
	/// a origin position and a destination position, to calculate efficiently
	/// the best path to be taken to reach the destination from the origin.
	/// </summary>
    public class Pathfinder
    {
        public Pathfinder()
        {
            root = new Node();
            stepQue = new List<Node>();
            bestPath = new Stack<Direction>();
        }

        /*********************************************************************/
        /* Class Types */
        /*********************************************************************/
		/// <summary>
		/// Generic Node structure, with cost and lastMove attributes.
		/// </summary>
		class Node
        {
            public Node()
            {
				childs = new List<Node>();
				totalCost = Int32.MaxValue;
				currentCost = 0;
            }

            public Node father;
            public List<Node> childs;
			public Direction lastMove;

			public Map.Pos pos;
            public int totalCost;
			public int currentCost;
        }

		/// <summary>
		/// Possibile directions to be taken in the given game world.
		/// </summary>
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        /*********************************************************************/
        /* Class Variables */
        /*********************************************************************/
        private Node root; // tree root
		private Node end;  // tree lowest/latest leaf

        private List<Node> stepQue; // possible nodes to follow in order to reach destination
        private Stack<Direction> bestPath; // stack of directions, informing the best path to destination

        private bool[,] visitedMap; // map informing which spaces have already been visited in search

		private bool foundTarget;
		private bool noSolution;

        /*********************************************************************/
        /* Class Methods */
        /*********************************************************************/
		/// <summary>
		/// Add node as a child of the given father, informing which direction
		/// was taken to reach such Node/Space.
		/// </summary>
        private void addTreeNode(Node father, Node son, Direction move)
        {
			son.lastMove = move;
			son.father = father;
            father.childs.Add(son);
        }

		/// <summary>
		/// Adds given Node to the stepQue.
		/// </summary>
        private void addQueNode(Node newNode)
        {
            stepQue.Add(newNode);
        }

		/// <summary>
		/// Initializes a boolean map structure with false values, since no spaces
		/// have been visited yet.
		/// </summary>
		/// <param name="map">Map on which the boolean map structure will be based.</param>
        private void initVisitedMap(Map map)
        {
            visitedMap = new bool[map.width, map.height];

            for (int i = 0; i < map.width; i++)
                for (int j = 0; j < map.height; j++)
                    visitedMap[i, j] = false;
        }

		/// <summary>
		/// Resets Pathfinder values, to make way for a new search.
		/// </summary>
		private void reset()
		{
			foundTarget = false;
			noSolution = false;

			root.childs.Clear();
			root.totalCost = 99;

			stepQue.Clear();
			bestPath.Clear();
		}

		/// <summary>
		/// Returns the Manhattan distance between two positions.
		/// </summary>
		/// <param name="origin">Origin position.</param>
		/// <param name="dest">Destination position.</param>
        private int calcPathCost(Map.Pos origin, Map.Pos dest)
        {
			return (Math.Abs(origin.x - dest.x) + Math.Abs(origin.y - dest.y));
        }

		/// <summary>
		/// Verifies if the target has been reached by the current node.
		/// If affirmative, a flag is set, as well as the destination leaf.
		/// </summary>
		/// <param name="map">Scene map.</param>
		/// <param name="current">Current node in question.</param>
		/// <param name="target">Target's position.</param>
		private void checkFound(Map map, Node current, Map.Pos target)
        {
			if (current.pos == target)
			{
				foundTarget = true;
				end = current;
			}
        }

		/// <summary>
		/// Performs a step, where every adjacent space to the current node is tested to see
		/// if it is a plausible candidate for a next direction to be followed in the
		/// pathfinding process.
		/// Is the node in a given direction is empty and has not been visited, it will
		/// be added to the the stepQue, as well as the main tree.
		/// If the target is reached in one of the directions adjacent to the current Node,
		/// the target is set as 'found'.
		/// </summary>
		/// <param name="map">Scene map.</param>
		/// <param name="current">Current node in question.</param>
		/// <param name="target">Target's position.</param>
		private void step(Map map, Node current, Map.Pos target)
        {
            // Test all adjacent nodes.
            // Left
			if ((map.getSpaceType(current.pos - Map.unitx) == Map.SpaceType.Empty) &&
				 (!visitedMap[current.pos.x - 1, current.pos.y]))
            {
                Node left = new Node();
				left.pos.x = current.pos.x - 1;
				left.pos.y = current.pos.y;
				left.currentCost++;
				left.totalCost = left.currentCost + calcPathCost(left.pos, target);
                addTreeNode(current, left, Direction.Left);
				checkFound( map, left, target);

				if (!foundTarget)
					addQueNode(left);
				else
					return;
            }

            // Right
			if ((map.getSpaceType(current.pos + Map.unitx) == Map.SpaceType.Empty) &&
				 (!visitedMap[current.pos.x + 1, current.pos.y]))
            {
                Node right = new Node();
				right.pos.x = current.pos.x + 1;
				right.pos.y = current.pos.y;
				right.currentCost++;
				right.totalCost = right.currentCost + calcPathCost(right.pos, target);
                addTreeNode(current, right, Direction.Right);
				checkFound(map, right, target);

                if (!foundTarget)
                    addQueNode(right);
				else
					return;
            }

            // Up
			if ((map.getSpaceType(current.pos - Map.unity) == Map.SpaceType.Empty) &&
				 (!visitedMap[current.pos.x, current.pos.y - 1]))
            {
                Node up = new Node();
				up.pos.x = current.pos.x;
				up.pos.y = current.pos.y - 1;
				up.currentCost++;
				up.totalCost = up.currentCost + calcPathCost(up.pos, target);
                addTreeNode(current, up, Direction.Up);
				checkFound(map, up, target);

				if (!foundTarget)
                    addQueNode(up);
				else
					return;
            }

            // Down
			if ((map.getSpaceType(current.pos + Map.unity) == Map.SpaceType.Empty) &&
				 (!visitedMap[current.pos.x, current.pos.y + 1]))
            {
                Node down = new Node();
				down.pos.x = current.pos.x;
				down.pos.y = current.pos.y + 1;
				down.currentCost++;
				down.totalCost = down.currentCost + calcPathCost(down.pos, target);
                addTreeNode(current, down, Direction.Down);
				checkFound(map, down, target);

				if (!foundTarget)
                    addQueNode(down);
				else
					return;
            }
        }

		/// <summary>
		/// Returns the Node with the lowest total cost in the stepQue.
		/// </summary>
        private Node findLowerCost()
        {
            int minCost = Int32.MaxValue;
			Node lowestCost = null;

			if (stepQue.Count == 0 )
			{
				noSolution = true;
				return null;
			}

            foreach (Node node in stepQue)
            {
                if (node.totalCost < minCost)
                {
                    lowestCost = node;
                    minCost = node.totalCost;
                }
            }

            return lowestCost;
        }

		/// <summary>
		/// Adds every Node from the tree's destination leaf up till the root, o a
		/// stack of Directions and returns the created stack.
		/// </summary>
		private void fillBestPath()
		{
			while (end.father != null)
			{
				bestPath.Push(end.lastMove);
				end = end.father;
			}
		}

		/// <summary>
		/// Finds and returns the best path to be followed from the origin postion,
		/// to the targets position, on the given map.
		/// Theoretically, I should only return the first position in the best path,
		/// since it is calculated on every change in the scene, but I prefered to leave
		/// it this way for now, in case I find a use for it later.
		/// </summary>
		/// <param name="map">Scene map.</param>
		/// <param name="origin">Origin's position.</param>
		/// <param name="target">Target's position.</param>
		public Stack<Pathfinder.Direction> findBestPath(Map map, Map.Pos origin, Map.Pos target)
		{
			// Setup visited map
			initVisitedMap(map);

			// reset pathfinders state
			reset();

			// Setup root
			root.pos = origin;
			visitedMap[root.pos.x, root.pos.y] = true;

			// performs inital step
			step(map, root, target);

			// if target is not found in the frist step, enter step loop
			Node nextTry;
			while (!foundTarget)
			{
				nextTry = findLowerCost();

				if (noSolution)
					return null;

				visitedMap[nextTry.pos.x, nextTry.pos.y] = true;
				stepQue.Remove(nextTry);
				step(map, nextTry, target);
			}

			// fill direction stack
			fillBestPath();

			return bestPath;
		}
    }
}
