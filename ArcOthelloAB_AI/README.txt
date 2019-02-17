OthelloAI

PROJECT: 	ArcOthelloAB

AUTHORS: 	Arzul Paul & Biloni Kim Aurore

DATE: 		17.02.18
ORGANISATION: 	HE-Arc Neuchâtel

Explication de la fonction d'évaluation:

Le score d'un noeud prend en compte trois paramètres, dans leur ordre d'importance:
1) le nombre de pions possédés par les joueurs
2) la mobilité des joueurs
3) la possession ou non des coins du plateau

Le score final prend en compte le résultat de es trois point et y applique des ratios
à chaqu'un afin de prioriser certains.

1)
Concernant le premier point, celui donne qu'une indication très temporaire de la situation
de jeu. Bien que le but final soit en effet d'avoir pplus de pion que l'adverssaire,
dans un jeu comme l'othello, la situation à tendence à souvent se renverser.

Le score est calculé de tel manière:
Score_NB_pion = NB_pion_joueur - NB_pion_adverssaire

2)
La mobilité est un paramètre essentiel dans les jeux de plateau.
Cela représente les possibilités offertes à un joueur.
Plus un joueur possède d'action possible, plus il a de choix stratégique.
Il est don important de faire en sorte de s'offrir un maximum de possibilité
en limitant celles de l'adverssaire.

Le score est calculé de tel manière:
Score_mobilité = NB_coups_possible_actuel - NB_coups_possible_noeud_précédent

3)
Dans le jeu de l'othello il est important de possèder un des coin du plateau.
En effet le coin de peuvent pas être volé par l'adverssaire après obention
et ceux-ci permettent un grande marche de manoeuvre pour le joueur qui les possèdes.

Le score est calculé de tel manière:
Score_NB_Coins = NB_coins_possédé - NB_de_coins_de_adverssaire

Enfin le score final vas prendre en compte ces paramètres.
Il est important de noter
que le score du nombre de pion vari en moyenne entre -10 et 10,
que le score de mobilité varie en moyenne entre -5 et 5,
et que le score de coin ne vari qu'entre -4 et 4.

Ansi le score final est:
score = score_NB_pion + score_mobilité * 5 + score_NB_coin * 200

Ainsi une très grande importance est mise au coins.
Si le jeu n'a pas encore atteint les coins, la mobilité prend tout de même le dessus.
Le nombre de pion, bien qu moindre reste tout de même pris en compte.

Enfin si le noeud à atteint la fin de partie,
le score deviendra la valeur maximal possible.
Int32.MaxValue, si le joueur gagne
Int32.MinValue, si le joueur perd

Ainsi l'IA va absolument éviter les situation perdante et
se dirigera toujours vers les situations gagnantes.



