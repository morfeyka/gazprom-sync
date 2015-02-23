using System;
using System.Diagnostics;

namespace HG.Base.NHibernate.DesignByContract
{
    public static class Check
    {
        private static bool _useAssertions;

        /// <summary>
        ///     Set this if you wish to use Trace Assert statements 
        ///     instead of exception handling. 
        ///     (The Check class uses exception handling by default.)
        /// </summary>
        public static bool UseAssertions
        {
            get { return _useAssertions; }

            set { _useAssertions = value; }
        }

        /// <summary>
        ///     Is exception handling being used?
        /// </summary>
        private static bool UseExceptions
        {
            get { return !_useAssertions; }
        }

        /// <summary>
        ///     Assertion check.
        /// </summary>
        public static void Assert(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new AssertionException(message);
            }
            else
                Trace.Assert(assertion, "Assertion: " + message);
        }

        /// <summary>
        ///     Assertion check.
        /// </summary>
        public static void Assert(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new AssertionException(message, inner);
            }
            else
                Trace.Assert(assertion, "Assertion: " + message);
        }

        /// <summary>
        ///     Assertion check.
        /// </summary>
        public static void Assert(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new AssertionException("Assertion failed.");
            }
            else
                Trace.Assert(assertion, "Assertion failed.");
        }

        /// <summary>
        ///     Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PostconditionException(message);
            }
            else
                Trace.Assert(assertion, "Postcondition: " + message);
        }

        /// <summary>
        ///     Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PostconditionException(message, inner);
            }
            else
                Trace.Assert(assertion, "Postcondition: " + message);
        }

        /// <summary>
        ///     Postcondition check.
        /// </summary>
        public static void Ensure(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PostconditionException("Postcondition failed.");
            }
            else
                Trace.Assert(assertion, "Postcondition failed.");
        }

        /// <summary>
        ///     Invariant check.
        /// </summary>
        public static void Invariant(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new InvariantException(message);
            }
            else
                Trace.Assert(assertion, "Invariant: " + message);
        }

        /// <summary>
        ///     Invariant check.
        /// </summary>
        public static void Invariant(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new InvariantException(message, inner);
            }
            else
                Trace.Assert(assertion, "Invariant: " + message);
        }

        /// <summary>
        ///     Invariant check.
        /// </summary>
        public static void Invariant(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new InvariantException("Invariant failed.");
            }
            else
                Trace.Assert(assertion, "Invariant failed.");
        }

        /// <summary>
        ///     Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion, string message)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PreconditionException(message);
            }
            else
                Trace.Assert(assertion, "Precondition: " + message);
        }

        /// <summary>
        ///     Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion, string message, Exception inner)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PreconditionException(message, inner);
            }
            else
                Trace.Assert(assertion, "Precondition: " + message);
        }

        /// <summary>
        ///     Precondition check - should run regardless of preprocessor directives.
        /// </summary>
        public static void Require(bool assertion)
        {
            if (UseExceptions)
            {
                if (!assertion)
                    throw new PreconditionException("Precondition failed.");
            }
            else
                Trace.Assert(assertion, "Precondition failed.");
        }
    }
}