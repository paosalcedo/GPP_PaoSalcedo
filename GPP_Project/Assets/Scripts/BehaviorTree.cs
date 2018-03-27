using System;

namespace BehaviorTree
{
    // The basic building block of a behavior tree is a node. The only requirement for a node is that
    // it report success or failure when it is updated. It takes a context variable so that leaf nodes
    // know who or what to operate on.
    public abstract class Node<T>
    {
        public abstract bool Update(T context); //an abstract bool that takes an object T
    }

    // Trees themselves are just a special kind of node that contain another node as a 'root'. The root
    // is where the execution of the tree always start.
    // Because trees are regular nodes they can be added to other trees as sub-trees.
    public class Tree<T> : Node<T>
    {
        private readonly Node<T> _root;

        public Tree(Node<T> root) { //pass it a node
            _root = root;
        }

        public override bool Update(T context)
        {
            return _root.Update(context);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // NODE TYPES
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // OUTER, LEAF OR 'ACTION' NODES
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Outer nodes are the 'leaves' of the tree and are where the action is. Literally.
    // The outer nodes is where the tree reaches a decision of what to do and actually
    // performs some aspect of the behavior (e.g. attacks an enemy, sounds an alarm, etc.)
    // Because it's where the game-specific actions you'll most likely make subclasses
    // for these but here is a basic version using delegates that allows you to write them 'in-line'
    public class Do<T> : Node<T>
    {
        public delegate bool NodeAction(T context);

        private readonly NodeAction _action;

        public Do(NodeAction action)
        {
            _action = action;
        }

        public override bool Update(T context)
        {
            return _action(context);
        }
    }

    // CONDITION
    // Conditions are just leaf nodes that test something. Similarly you can create subclasses for these
    // or else use the version below to write them inline.
    public class Condition<T> : Node<T>
    {
        private readonly Predicate<T> _condition;

        public Condition(Predicate<T> condition)
        {
            _condition = condition;
        }

        public override bool Update(T context)
        {
            return _condition(context);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // INNER, COMPOSITE OR 'DECISION' NODES
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // What defines an inner node is that it has a set of 'child' or 'branch' nodes.
    // These are the nodes that define the structure and logic of three but they
    // won't actually *do* anything
    public abstract class BranchNode<T> : Node<T>
    {
        protected Node<T>[] Children { get; private set; }

        protected BranchNode(params Node<T>[] children)
        {
            Children = children;
        }
    }


    // SELECTOR
    // Succeeds when ONE of its children succeeded
    // Fails when ALL of its children failed
    // Used to select from a series of ranked options (e.g. Try to cast a spell, if not close for a melee attack)
    public class Selector<T> : BranchNode<T>
    {
        public Selector(params Node<T>[] children) : base(children) {}

        public override bool Update(T context)
        {
            foreach (var child in Children)
            {
                if (child.Update(context)) return true;
            }
            return false;
        }
    }

    // SEQUENCE
    // Succeeds when ALL of its children succeeded
    // Fails when ONE of its children failed
    // Used for things like checklists (e.g. If there's food nearby, and if I'm hungry, then eat the food)
    public class Sequence<T> : BranchNode<T>
    {
        public Sequence(params Node<T>[] children) : base(children) {}

        public override bool Update(T context)
        {
            foreach (var child in Children)
            {
                if (!child.Update(context)) return false;
            }
            return true;
        }
    }

    // DECORATOR
    // Decorators are nodes that act as a 'modifier' for another node
    // This is a base class that just holds a reference to the 'modified' or 'decorated' node
    public abstract class Decorator<T> : Node<T>
    {
        protected Node<T> Child { get; private set; }

        protected Decorator(Node<T> child)
        {
            Child = child;
        }
    }

    // A common example of a Decorator node is a Not or Negate node
    // that inverts the result of another node. For example if you have a node that checks if there's any friends
    // nearby you may need to run some logic when there are NO friends nearby. A Not node allows you to do that
    // without having to create a different node
    public class Not<T> : Decorator<T>
    {
        public Not(Node<T> child) : base(child) {}

        public override bool Update(T context)
        {
            return !Child.Update(context);
        }
    }


}