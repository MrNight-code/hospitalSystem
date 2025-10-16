using HospitalData.DTOs;

namespace HospitalData.Services
{
    // Este servicio guardará la información del usuario que ha iniciado sesión.
    public class UserSessionService
    {
        // Propiedad para almacenar los datos del usuario actual.
        // Es 'nullable' (?), lo que significa que puede ser nulo si nadie ha iniciado sesión.
        public AuthenticatedUser? CurrentUser { get; private set; }

        // Un evento que se disparará cada vez que el estado de la sesión cambie (login o logout).
        // Otros componentes se "suscribirán" a este evento para actualizarse automáticamente.
        public event Action? OnChange;

        // Método para "iniciar sesión" en el servicio. Guarda los datos del usuario.
        public void Login(AuthenticatedUser user)
        {
            CurrentUser = user;
            NotifyStateChanged();
        }

        // Método para "cerrar sesión". Limpia los datos del usuario.
        public void Logout()
        {
            CurrentUser = null;
            NotifyStateChanged();
        }

        // Método privado que dispara el evento OnChange para notificar a los suscriptores.
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}