# TelematiqueTP3

## API
### Groupes
- [x] /groups/ (GET) : Liste de tous les groupes
- [x] /groups/ (POST) : Création d'un groupe

- [x] /groups/:group/users (GET) : Liste les utilisateurs du groupe
- [ ] /groups/:group/users/:id (POST) : Ajoute l'utilisateur au groupe                              *same user
- [ ] /groups/:group/users/:id (PUT) : Modifie les permissions de l'utilisateur dans le groupe      *admin
- [ ] /groups/:group/users/:id (DELETE) : Supprime l'utilisateur du groupe du groupe                *admin

- [ ] /groups/:group/invites/:user (POST) : Invite l'utilisateur à rejoindre le groupe              *admin

### Utilisateurs
- [x] /users (GET) : Liste les utilisateurs
- [ ] /users/:user/invites (GET) : Récupère les invitations en attente                              *same user
- [ ] /users/:user/invites/:group (DELETE) : Refuse de rejoindre le groupe                          *same user

- [x] /users/:user/groups (GET) : Liste les groupes auquel l'utilisateur appartient                 *same user

- [x] /users/authenticate (POST) : Authentification

### Setup
- [x] /setup/reset (GET) : Réinitialise la base de données
