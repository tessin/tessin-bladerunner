namespace Tessin.Bladerunner.Grid
{
    public interface IGridRenderer<T>
    {
        object Render(EntityGrid<T> entityGrid);
    }
}