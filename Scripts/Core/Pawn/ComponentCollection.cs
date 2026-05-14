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
    private Dictionary<Type, List<PawnComponent>> Children { get; set; } = new();
    private HashSet<PawnComponent> Registry { get; set; } = new();

    /* Constructors. */
    public ComponentCollection(Pawn pawn)
    {
        Pawn = pawn;
        Scan(pawn);
    }

    /* Public methods. */
    /// <summary>
    /// Rebuild the collection from the root node's current children.
    /// </summary>
    public void Rebuild()
    {
        Children.Clear();
        Scan(Pawn);
    }

    /// <summary>
    /// Register a component.
    /// </summary>
    public void Add(PawnComponent component)
    {
        if (component == null)
            throw new ArgumentNullException(nameof(component));

        // Add node to list(s).
        Type type = component.GetType();
        while (type != null && typeof(PawnComponent).IsAssignableFrom(type))
        {
            // Get or create the component type's list.
            if (!Children.TryGetValue(type, out var list))
            {
                list = new List<PawnComponent>();
                Children[type] = list;
            }

            // Add the component to its list.
            if (!Registry.Contains(component))
                list.Add(component);

            // Continue with base type.
            type = type.BaseType;
        }

        // Add to registry.
        Registry.Add(component);
    }

    /// <summary>
    /// Unregister a component.
    /// </summary>
    public void Remove(PawnComponent component)
    {
        if (component == null)
            throw new ArgumentNullException(nameof(component));

        if (!Registry.Contains(component))
            throw new ArgumentException($"The component was not a member of pawn '{Pawn.Name}'.", nameof(component));

        // Remove node from list(s).
        Type type = component.GetType();
        while (type != null && typeof(PawnComponent).IsAssignableFrom(type))
        {
            // Remove from the component type's list.
            if (Children.TryGetValue(type, out var list))
                list.Remove(component);

            // Continue with base type.
            type = type.BaseType;
        }

        // Remove from registry.
        Registry.Remove(component);
    }

    /// <summary>
    /// Get the number of components of some type (including derived types).
    /// </summary>
    public int Count<T>()
        where T : PawnComponent
    {
        return Children.TryGetValue(typeof(T), out var list)
            ? list.Count
            : 0;
    }

    /// <summary>
    /// Get a component of some type (including derived types).
    /// </summary>
    public T Get<T>()
        where T : PawnComponent
    {
        if (Children.TryGetValue(typeof(T), out var list) && list.Count > 0)
            return (T)list[0];

        return null;
    }

    /// <summary>
    /// Get a component of some type (including derived types).
    /// </summary>
    public T GetAt<T>(int index)
        where T : PawnComponent
    {
        if (Children.TryGetValue(typeof(T), out var list) && index >= 0 && index < list.Count)
            return (T)list[index];

        return null;
    }

    /// <summary>
    /// Get the first active component of some type (including derived types).
    /// </summary>
    public T GetFirstActive<T>()
        where T : PawnComponent
    {
        if (!Children.TryGetValue(typeof(T), out var list))
            return null;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IsActive(Pawn))
                return (T)list[i];
        }

        return null;
    }

    /* Private methods. */
    /// <summary>
    /// Recursively collect a node's child pawn component. Is blocked by child pawns.
    /// </summary>
    private void Scan(Node root)
    {
        // Add lists for each type of component.
        for (int i = 0; i < root.GetChildCount(); i++)
        {
            Node node = root.GetChild(i);
            if (node is PawnComponent component)
            {
                GD.Print("- Adding: " + node.Name);
                Add(component);
            }

            if (node is not Pawn pawn)
                Scan(node);
        }
    }

}