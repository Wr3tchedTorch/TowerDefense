using Godot;

namespace TowerDefense.scripts.upgrade
{
    public interface IUpgrade<T>
    {
        public void ApplyUpgrade(T target);
    }
}