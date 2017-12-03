using Microsoft.Xna.Framework;

namespace ShooterAttack
{
    class PathNode
    {
        public PathNode ParentNode;
        public PathNode EndNode;
        private Vector2 gridLocation;
        public float TotalCost;
        public float DirectCost;

        public Vector2 GridLocation
        {
            get { return gridLocation; }
            set { gridLocation = new Vector2((float)MathHelper.Clamp(value.X, 0f, (float)Map.MapWidth), (float)MathHelper.Clamp(value.Y, 0f, (float)Map.MapHeight)); }
        }

        public int GridX
        {
            get { return (int)gridLocation.X; }
        }

        public int GridY
        {
            get { return (int)gridLocation.Y; }
        }

        public PathNode(PathNode parentNode, PathNode endNode, Vector2 gridLocation, float cost)
        {
            ParentNode = parentNode;
            GridLocation = gridLocation;
            EndNode = endNode;
            DirectCost = cost;
            if (!(endNode == null))
                TotalCost = DirectCost + LinearCost();
        }

        public float LinearCost()
        {
            return (Vector2.Distance(EndNode.GridLocation, this.GridLocation));
        }

        public bool IsEqualToNode(PathNode node)
        {
            return (GridLocation == node.GridLocation);
        }
    }
}
