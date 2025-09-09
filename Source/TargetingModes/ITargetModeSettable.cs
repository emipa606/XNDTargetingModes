namespace TargetingModes;

public interface ITargetModeSettable
{
    TargetingModeDef GetTargetingMode();

    void SetTargetingMode(TargetingModeDef targetMode);
}