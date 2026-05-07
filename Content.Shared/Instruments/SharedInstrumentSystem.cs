namespace Content.Shared.Instruments;

public abstract class SharedInstrumentSystem : EntitySystem
{
    public abstract bool ResolveInstrument(EntityUid uid, ref SharedInstrumentComponent? component);

    public virtual void SetupRenderer(EntityUid uid, bool fromStateChange, SharedInstrumentComponent? instrument = null)
    {
    }

    public virtual void EndRenderer(EntityUid uid, bool fromStateChange, SharedInstrumentComponent? instrument = null)
    {
    }

    public void SetInstrumentProgram(EntityUid uid, SharedInstrumentComponent component, byte program, byte bank)
    {
        component.InstrumentBank = bank;
        component.InstrumentProgram = program;
        Dirty(uid, component);
    }

    public void SetFilteredChannel(EntityUid uid, SharedInstrumentComponent component, int channel, bool filtered)
    {
        if (channel < 0 || channel >= component.FilteredChannels.Length)
            return;

        if (component.FilteredChannels[channel] == filtered)
            return;

        component.FilteredChannels[channel] = filtered;
        Dirty(uid, component);
    }
}
