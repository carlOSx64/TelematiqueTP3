# TelematiqueTP3

## API
### Groupes
- [x] /groups/ (GET) : Liste de tous les groupes
- [x] /groups/ (POST) : Création d'un groupe

- [x] /groups/:group/users (GET) : Liste les utilisateurs du groupe
- [x] /groups/:group/users/:id (POST) : Ajoute l'utilisateur au groupe                              *same user
- [x] /groups/:group/users/:id (PUT) : Modifie les permissions de l'utilisateur dans le groupe      *admin
- [x] /groups/:group/users/:id (DELETE) : Supprime l'utilisateur du groupe du groupe                *admin

- [x] /groups/:group/invitations/:user (POST) : Invite l'utilisateur à rejoindre le groupe          *admin
- [x] /groups/:group/invitations/:user (PUT) : Accepter ou refuser de rejoindre le groupe           *same user

### Utilisateurs
- [x] /users (GET) : Liste les utilisateurs

- [x] /users/:user/invitations (GET) : Récupère les invitations en attente                          *same user

- [x] /users/:user/groups (GET) : Liste les groupes auquel l'utilisateur appartient                 *same user

- [x] /users/authenticate (POST) : Authentification

### Files
- [x] /files (POST) : Ajoute un fichier. Besoin de `name` et `content`
- [x] /files/{someId} (DELETE) : Delete un fichier


### Setup
- [x] /setup/reset (GET) : Réinitialise la base de données
