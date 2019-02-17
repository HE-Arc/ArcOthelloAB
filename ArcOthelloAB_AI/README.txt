OthelloAI

PROJECT: 	ArcOthelloAB

AUTHORS: 	Arzul Paul & Biloni Kim Aurore

DATE: 		17.02.18
ORGANISATION: 	HE-Arc Neuch�tel

Explication de la fonction d'�valuation:

Le score d'un noeud prend en compte trois param�tres, dans leur ordre d'importance:
1) le nombre de pions poss�d�s par les joueurs
2) la mobilit� des joueurs
3) la possession ou non des coins du plateau

Le score final prend en compte le r�sultat de es trois point et y applique des ratios
� chaqu'un afin de prioriser certains.

1)
Concernant le premier point, celui donne qu'une indication tr�s temporaire de la situation
de jeu. Bien que le but final soit en effet d'avoir pplus de pion que l'adverssaire,
dans un jeu comme l'othello, la situation � tendence � souvent se renverser.

Le score est calcul� de tel mani�re:
Score_NB_pion = NB_pion_joueur - NB_pion_adverssaire

2)
La mobilit� est un param�tre essentiel dans les jeux de plateau.
Cela repr�sente les possibilit�s offertes � un joueur.
Plus un joueur poss�de d'action possible, plus il a de choix strat�gique.
Il est don important de faire en sorte de s'offrir un maximum de possibilit�
en limitant celles de l'adverssaire.

Le score est calcul� de tel mani�re:
Score_mobilit� = NB_coups_possible_actuel - NB_coups_possible_noeud_pr�c�dent

3)
Dans le jeu de l'othello il est important de poss�der un des coin du plateau.
En effet le coin de peuvent pas �tre vol� par l'adverssaire apr�s obention
et ceux-ci permettent un grande marche de manoeuvre pour le joueur qui les poss�des.

Le score est calcul� de tel mani�re:
Score_NB_Coins = NB_coins_poss�d� - NB_de_coins_de_adverssaire

Enfin le score final vas prendre en compte ces param�tres.
Il est important de noter
que le score du nombre de pion vari en moyenne entre -10 et 10,
que le score de mobilit� varie en moyenne entre -5 et 5,
et que le score de coin ne vari qu'entre -4 et 4.

Ansi le score final est:
score = score_NB_pion + score_mobilit� * 5 + score_NB_coin * 200

Ainsi une tr�s grande importance est mise au coins.
Si le jeu n'a pas encore atteint les coins, la mobilit� prend tout de m�me le dessus.
Le nombre de pion, bien qu moindre reste tout de m�me pris en compte.

Enfin si le noeud � atteint la fin de partie,
le score deviendra la valeur maximal possible.
Int32.MaxValue, si le joueur gagne
Int32.MinValue, si le joueur perd

Ainsi l'IA va absolument �viter les situation perdante et
se dirigera toujours vers les situations gagnantes.



