using System;
using System.Security;

namespace Donker.ConsoleUtils.PasswordBuilders
{
    /// <summary>
    /// Simple class for building a secure password.
    /// </summary>
    public class SecurePasswordBuilder : IPasswordBuilder<SecureString>, IDisposable
    {
        private SecureString _secureString;

        /// <summary>
        /// Gets the length of the password.
        /// </summary>
        public int Length
        {
            get
            {
                ThrowExceptionWhenDisposed();
                return _secureString.Length;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SecurePasswordBuilder"/>.
        /// </summary>
        public SecurePasswordBuilder()
        {
            _secureString = new SecureString();
        }

        /// <summary>
        /// Adds a character to the password.
        /// </summary>
        /// <param name="c">The character to add.</param>
        public void AddChar(char c)
        {
            ThrowExceptionWhenDisposed();
            _secureString.AppendChar(c);
        }

        /// <summary>
        /// Removes the last character from the current password.
        /// </summary>
        public void Backspace()
        {
            ThrowExceptionWhenDisposed();

            if (_secureString.Length == 0)
                return;

            _secureString.RemoveAt(_secureString.Length - 1);
        }

        /// <summary>
        /// Returns the fully built password.
        /// </summary>
        /// <returns>The result as a <see cref="SecureString"/> object.</returns>
        public SecureString GetResult()
        {
            ThrowExceptionWhenDisposed();
            return _secureString.Copy();
        }

        #region Generated Dispose members

        /// <summary>
        /// Gets whether this instance is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Override this method to add your own dispose logic.
        /// </summary>
        /// <param name="disposing">If the instance is being disposed after the public <see cref="Dispose()"/> method has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            if (disposing)
                _secureString.Dispose();

            _secureString = null;
        }

        /// <summary>
        /// Call this method to thrown an <see cref="ObjectDisposedException"/> when this instance has been disposed.
        /// </summary>
        protected void ThrowExceptionWhenDisposed()
        {
            if (!IsDisposed)
                return;

            Type thisType = typeof (SecurePasswordBuilder);
            throw new ObjectDisposedException(thisType.Name);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources for this <see cref="SecurePasswordBuilder"/> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        /// Destroys this <see cref="SecurePasswordBuilder"/> instance.
        /// </summary>
        ~SecurePasswordBuilder()
        {
            Dispose(false);
        }

        #endregion
    }
}