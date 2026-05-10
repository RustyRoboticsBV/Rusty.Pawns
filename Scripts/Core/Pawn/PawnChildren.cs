using Godot;
using System;
using System.Collections.Generic;

namespace Rusty.Pawns;

/// <summary>
/// A database of pawn components that are children of some parent node.
/// </summary>
public class PawnChildren
{
    /* Public properties. */
    public Node Root { get; private set; }

    /* Private properties. */
    private Dictionary<Type, List<PawnComponent>> Children { get; set; }

    /* Constructors. */
    public PawnChildren(Node root)
    {
        Root = root;
        Ensure();
    }

    /* Public methods. */
    public int Count<T>()
    {
        return Children[typeof(T)].Count;
    }

    public T GetAt<T>(int index)
        where T : PawnComponent
    {
        return (T)Children[typeof(T)][index];
    }

    public void Clear()
    {
        Children = null;
        Ensure();
    }

    /* Private methods. */
    private void Ensure()
    {
        // Do nothing if the child dictionary was already created.
        if (Children == null)
            return;

        // Add list for root type.
        if (!Children.ContainsKey(typeof(PawnComponent)))
            Children.Add(typeof(PawnComponent), new());

        // Add lists for each type of component.
        for (int i = 0; i < Root.GetChildCount(); i++)
        {
            Node node = Root.GetChild(i);
            if (node is PawnComponent component)
            {
                // Add list for child type.
                Type type = node.GetType();
                if (!Children.ContainsKey(type))
                    Children.Add(type, new());

                // Add node to lists.
                Children[typeof(PawnComponent)].Add(component);
                Children[type].Add(component);
            }
        }
    }
}