using System;
using System.Collections.Generic;
using Godot;

namespace Rusty.Pawns;

/// <summary>
/// A database of pawn components that are children of some parent node.
/// </summary>
public class ComponentCollection
{
    /* Public properties. */
    public Pawn Pawn { get; private set; }

    /* Private properties. */
    private Dictionary<Type, List<PawnComponent>> Children { get; set; }

    /* Constructors. */
    public ComponentCollection(Pawn pawn)
    {
        Pawn = pawn;
        Create();
    }

    /* Public methods. */
    /// <summary>
    /// Get the number of components of some type.
    /// </summary>
    public int Count<T>()
    {
        return Children[typeof(T)].Count;
    }

    /// <summary>
    /// Get a component of some type.
    /// </summary>
    public T Get<T>()
        where T : PawnComponent
    {
        return (T)Children[typeof(T)][0];
    }

    /// <summary>
    /// Get a component of some type.
    /// </summary>
    public T GetAt<T>(int index)
        where T : PawnComponent
    {
        return (T)Children[typeof(T)][index];
    }

    /// <summary>
    /// Get the first active component of some type.
    /// </summary>
    public T GetFirstActive<T>()
        where T : PawnComponent
    {
        List<PawnComponent> components = Children[typeof(T)];
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i].IsActive(Pawn))
                return (T)components[i];
        }
        return null;
    }

    /* Private methods. */
    /// <summary>
    /// Create the collection.
    /// </summary>
    private void Create()
    {
        Type baseType = typeof(PawnComponent);

        // Do nothing if the child dictionary was already created.
        if (Children == null)
            return;

        // Add list for base component type.
        if (!Children.ContainsKey(baseType))
            Children.Add(baseType, new());

        // Add lists for each type of component.
        for (int i = 0; i < Pawn.GetChildCount(); i++)
        {
            Node node = Pawn.GetChild(i);
            if (node is PawnComponent component)
            {
                // Add list for child type.
                Type type = node.GetType();
                if (!Children.ContainsKey(type))
                    Children.Add(type, new());

                // Add node to lists.
                Children[baseType].Add(component);
                Children[type].Add(component);
            }
        }
    }
}