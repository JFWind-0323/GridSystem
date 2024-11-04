public interface ICell
{
    bool CheckTarget(int targetX, int targetY);
    void MoveToTarget(int targetX, int targetY);
}