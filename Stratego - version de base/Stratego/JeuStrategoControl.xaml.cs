using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stratego
{
    /// <summary>
    /// Logique d'interaction pour JeuStrategoControl.xaml
    /// </summary>
    public partial class JeuStrategoControl : UserControl
    {
        #region Static

        private const int TAILLE_CASES_GRILLE = 50;

        #endregion

        public GrilleJeu GrillePartie { get; private set; }

        private List<List<Image>> GrillePieces { get; set; }

        private Rectangle SelectionActive { get; set; }

        public Couleur TourJeu { get; private set; }

        public Couleur CouleurJoueur { get; private set; }

        public List<Piece> LstPiecesEliminees { get; set; }

        private MainWindow mainWindow { get; set; }

        #region Code relié au patron observateur

        List<IObserver<JeuStrategoControl>> observers;

        // Oui, une classe privée (et interne).
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<JeuStrategoControl>> _observers;
            private IObserver<JeuStrategoControl> _observer;

            public Unsubscriber(List<IObserver<JeuStrategoControl>> observers, IObserver<JeuStrategoControl> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }

        public IDisposable Subscribe(IObserver<JeuStrategoControl> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        private void Notify()
        {
            foreach (IObserver<JeuStrategoControl> ob in observers)
            {
                ob.OnNext(this);
            }
        }
        #endregion

        private IA_Stratego IA { get; set; }

        /// <summary>
        /// Initialise l'interface du jeu pour une nouvelle partie
        /// </summary>
        /// <param name="couleurJoueur">Couleur du joueur choisi lors de la configuration du jeu</param>
        /// <param name="TabPiecePositionJoueur">Les pièces positionnées par le joueur</param>
        /// <param name="mainWindow">Pour intérargir les fonctionnalités du MainWindow</param>
        public JeuStrategoControl(Couleur couleurJoueur, Piece[,] TabPiecePositionJoueur, MainWindow mainWindow)
        {
            CouleurJoueur = couleurJoueur;
            this.mainWindow = mainWindow;
            LstPiecesEliminees = new List<Piece>();

            InitializeComponent();

            GrillePartie = new GrilleJeu();

            DiviserGrilleJeu();
            ColorerGrilleJeu();
            DefinirZoneSelectionGrille();
            InitialiserSelectionActive();

            // Initialise la liste d'observateurs.
            observers = new List<IObserver<JeuStrategoControl>>();

            // Initialiser l'IA.
            if (CouleurJoueur != Couleur.Rouge)
            {
                IA = new IA_Stratego(this, Couleur.Rouge);
                Thread executionIA = new Thread(LancerIA);
                executionIA.Start();
            }
            else
                IA = new IA_Stratego(this, Couleur.Bleu);

            PositionnerPieces(TabPiecePositionJoueur);
            InitialiserAffichagePieces();

            #region Tests

            // Code des tests initiaux.
            /*
            ReponseDeplacement deplacement;

            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(0, 6), new Coordonnee(0, 5)); // Deplacement

            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(0, 5), new Coordonnee(-1, 5)); // Coord invalide
            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(2, 6), new Coordonnee(2, 5)); // Lac

            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(2, 6), new Coordonnee(3, 6)); // Piece vs sa propre couleur

            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 6), new Coordonnee(1, 5));
            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 5), new Coordonnee(1, 4));
            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 4), new Coordonnee(1, 3)); // Prise par attaquant

            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 3), new Coordonnee(1, 2));
            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 2), new Coordonnee(1, 1));
            // deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 1), new Coordonnee(1, 0)); // 2 pièces éliminées
            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(1, 1), new Coordonnee(2, 1));
            deplacement = GrillePartie.ResoudreDeplacement(new Coordonnee(2, 1), new Coordonnee(2, 0)); // Attaquant éliminé
            */

            #endregion

            TourJeu = Couleur.Rouge;


        }

        /// <summary>
        /// Cette méthode existe principalement pour que le jeu soit testable.
        /// On ne veut évidemment pas toujours commencer une partie avec exactement les même positions.
        /// </summary>
        /// <param name="TabPiecePositionJoueur">Les pièces positionnées par le joueur dans la configuration de partie</param>
        private void PositionnerPieces(Piece[,] TabPiecePositionJoueur)
        {
            List<Piece> piecesRouges = new List<Piece>();
            List<Piece> piecesBleues = new List<Piece>();

            
            if (TabPiecePositionJoueur.GetValue(0, 0) == null)//  Permet de faire des tests
            {

                piecesRouges = new List<Piece>() { new Marechal(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Demineur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Capitaine(Couleur.Rouge)
                                                        , new Sergent(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Colonel(Couleur.Rouge), new Colonel(Couleur.Rouge), new General(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Sergent(Couleur.Rouge), new Bombe(Couleur.Rouge), new Bombe(Couleur.Rouge), new Lieutenant(Couleur.Rouge)
                                                        , new Commandant(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Commandant(Couleur.Rouge), new Espion(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Bombe(Couleur.Rouge), new Sergent(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Eclaireur(Couleur.Rouge)
                                                        , new Commandant(Couleur.Rouge), new Demineur(Couleur.Rouge), new Demineur(Couleur.Rouge), new Demineur(Couleur.Rouge), new Sergent(Couleur.Rouge), new Bombe(Couleur.Rouge), new Drapeau(Couleur.Rouge), new Bombe(Couleur.Rouge), new Bombe(Couleur.Rouge), new Demineur(Couleur.Rouge)
                                                        };

                piecesBleues = new List<Piece>() { new Commandant(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Demineur(Couleur.Bleu), new Demineur(Couleur.Bleu), new Demineur(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu), new Bombe(Couleur.Bleu), new Drapeau(Couleur.Bleu)
                                                        , new Capitaine(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Sergent(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu), new Bombe(Couleur.Bleu)
                                                        , new Eclaireur(Couleur.Bleu), new Commandant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Marechal(Couleur.Bleu), new Commandant(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Demineur(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu)
                                                        , new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Colonel(Couleur.Bleu), new Demineur(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Colonel(Couleur.Bleu), new Espion(Couleur.Bleu), new General(Couleur.Bleu), new Bombe(Couleur.Bleu)
                                                        };
                if(CouleurJoueur == Couleur.Rouge)
                    IA.PreparerPositionsPieces(piecesBleues);
                else
                    IA.PreparerPositionsPieces(piecesRouges);
            }
            else
            {
                if (CouleurJoueur == Couleur.Rouge)
                {
                    foreach (Piece pieceTab in TabPiecePositionJoueur)
                    {
                        piecesRouges.Add(pieceTab);
                    }
                    piecesBleues = new List<Piece>() { new Commandant(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Demineur(Couleur.Bleu), new Demineur(Couleur.Bleu), new Demineur(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu), new Bombe(Couleur.Bleu), new Drapeau(Couleur.Bleu)
                                                        , new Capitaine(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Sergent(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu), new Bombe(Couleur.Bleu)
                                                        , new Eclaireur(Couleur.Bleu), new Commandant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Marechal(Couleur.Bleu), new Commandant(Couleur.Bleu), new Capitaine(Couleur.Bleu), new Demineur(Couleur.Bleu), new Bombe(Couleur.Bleu), new Sergent(Couleur.Bleu)
                                                        , new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Colonel(Couleur.Bleu), new Demineur(Couleur.Bleu), new Lieutenant(Couleur.Bleu), new Eclaireur(Couleur.Bleu), new Colonel(Couleur.Bleu), new Espion(Couleur.Bleu), new General(Couleur.Bleu), new Bombe(Couleur.Bleu)
                                                        };
                    IA.PreparerPositionsPieces(piecesBleues);
                }
                else
                {
                    foreach (Piece pieceTab in TabPiecePositionJoueur)
                    {
                        piecesBleues.Add(pieceTab);
                    }

                    piecesRouges = new List<Piece>() { new Marechal(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Demineur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Capitaine(Couleur.Rouge)
                                                        , new Sergent(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Colonel(Couleur.Rouge), new Colonel(Couleur.Rouge), new General(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Sergent(Couleur.Rouge), new Bombe(Couleur.Rouge), new Bombe(Couleur.Rouge), new Lieutenant(Couleur.Rouge)
                                                        , new Commandant(Couleur.Rouge), new Eclaireur(Couleur.Rouge), new Commandant(Couleur.Rouge), new Espion(Couleur.Rouge), new Capitaine(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Bombe(Couleur.Rouge), new Sergent(Couleur.Rouge), new Lieutenant(Couleur.Rouge), new Eclaireur(Couleur.Rouge)
                                                        , new Commandant(Couleur.Rouge), new Demineur(Couleur.Rouge), new Demineur(Couleur.Rouge), new Demineur(Couleur.Rouge), new Sergent(Couleur.Rouge), new Bombe(Couleur.Rouge), new Drapeau(Couleur.Rouge), new Bombe(Couleur.Rouge), new Bombe(Couleur.Rouge), new Demineur(Couleur.Rouge)
                                                        };
                    IA.PreparerPositionsPieces(piecesRouges);
                }
            }


            if (CouleurJoueur == Couleur.Rouge)
            {
                GrillePartie.PositionnerPieces(piecesRouges, true);
                GrillePartie.PositionnerPieces(piecesBleues, false);

            }
            else
            {
                GrillePartie.PositionnerPieces(piecesBleues, true);
                GrillePartie.PositionnerPieces(piecesRouges, false);
            }



        }
        /// <summary>
        /// Crée la structure de la grid en grille de 10*10
        /// </summary>
        private void DiviserGrilleJeu()
        {
            ColumnDefinition colonneDef;
            RowDefinition ligneDef;

            for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
            {
                colonneDef = new ColumnDefinition();
                colonneDef.Width = new GridLength(TAILLE_CASES_GRILLE);
                grdPartie.ColumnDefinitions.Add(colonneDef);

                ligneDef = new RowDefinition();
                ligneDef.Height = new GridLength(TAILLE_CASES_GRILLE);
                grdPartie.RowDefinitions.Add(ligneDef);
            }
        }

        /// <summary>
        /// Ajoute de la coloration à chaque case selon son type de terrain et ajoute des lignes verticalement et horizontalement.
        /// </summary>
        private void ColorerGrilleJeu()
        {
            Rectangle ligne;

            for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
            {
                grdPartie.Children.Add(CreerLigneGrille(i, true));

                for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
                {
                    grdPartie.Children.Add(CreerFondCase(i, j));

                    if (i == 0)
                    {
                        grdPartie.Children.Add(CreerLigneGrille(j, false));
                    }
                }
            }

            ligne = CreerLigneGrille(0, true);
            ligne.HorizontalAlignment = HorizontalAlignment.Left;
            grdPartie.Children.Add(ligne);

            ligne = CreerLigneGrille(0, false);
            ligne.VerticalAlignment = VerticalAlignment.Top;
            grdPartie.Children.Add(ligne);
        }

        /// <summary>
        /// Crée une ligne selon la position verticale ou horizontale demandée.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="estColonne">Indique si la ligne doit être créée horizontalement ou verticalement</param>
        /// <returns></returns>
        private Rectangle CreerLigneGrille(int position, bool estColonne)
        {
            Rectangle ligne = new Rectangle();
            ligne.Fill = Brushes.Gainsboro;
            Grid.SetZIndex(ligne, 1);

            if (estColonne)
            {
                ligne.Width = 1;
                ligne.Height = 10 * TAILLE_CASES_GRILLE;
                ligne.HorizontalAlignment = HorizontalAlignment.Right;
                Grid.SetColumn(ligne, position);
                Grid.SetRow(ligne, 0);
                Grid.SetRowSpan(ligne, 10);
            }
            else
            {
                ligne.Width = 10 * TAILLE_CASES_GRILLE;
                ligne.Height = 1;
                ligne.VerticalAlignment = VerticalAlignment.Bottom;
                Grid.SetColumn(ligne, 0);
                Grid.SetColumnSpan(ligne, 10);
                Grid.SetRow(ligne, position);
            }

            return ligne;
        }

        /// <summary>
        /// Crée un fond selon le type de terrain que la case est. 
        /// </summary>
        /// <param name="colonne">L'axe x dans lequel la case est positionnée.</param>
        /// <param name="rangee">L'axe y dans lequel la case est positionnée.</param>
        /// <returns></returns>
        private Rectangle CreerFondCase(int colonne, int rangee)
        {
            Rectangle rect = new Rectangle();

            rect.Width = TAILLE_CASES_GRILLE;
            rect.Height = TAILLE_CASES_GRILLE;

            if (GrillePartie.EstCoordonneeLac(new Coordonnee(colonne, rangee)))
            {
                rect.Fill = Brushes.CornflowerBlue;
            }
            else
            {
                rect.Fill = Brushes.OliveDrab;
            }

            Grid.SetZIndex(rect, 0);
            Grid.SetColumn(rect, colonne);
            Grid.SetRow(rect, rangee);

            return rect;
        }

        /// <summary>
        /// Crée un rectangle de sélection pour chaque case et y insère une fonction pour activé la sélection de la case
        /// lorsque le joueur clic avec la souris.
        /// </summary>
        private void DefinirZoneSelectionGrille()
        {
            Rectangle rect;

            for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
            {
                for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
                {
                    rect = new Rectangle();

                    rect.Width = TAILLE_CASES_GRILLE;
                    rect.Height = TAILLE_CASES_GRILLE;
                    rect.Fill = Brushes.Transparent;
                    Grid.SetZIndex(rect, 5);
                    Grid.SetColumn(rect, i);
                    Grid.SetRow(rect, j);

                    grdPartie.Children.Add(rect);

                    rect.MouseLeftButtonUp += ResoudreSelectionCase;
                }

            }

        }

        /// <summary>
        /// Crée le rectangle de sélection visuel pour indiquer au joueur qu'il a bien sélectionné une tel case.
        /// </summary>
        private void InitialiserSelectionActive()
        {
            SelectionActive = new Rectangle();

            SelectionActive.Width = TAILLE_CASES_GRILLE;
            SelectionActive.Height = TAILLE_CASES_GRILLE;
            SelectionActive.Fill = Brushes.Yellow;
            SelectionActive.Opacity = 0.4;

            Grid.SetZIndex(SelectionActive, 3);
        }

        /// <summary>
        /// Place l'affichage de chaque pièce dans la gille du jeu (la grid)
        /// </summary>
        private void InitialiserAffichagePieces()
        {
            Coordonnee position;
            Image ImageAffichage;

            GrillePieces = new List<List<Image>>();

            for (int i = 0; i < GrilleJeu.TAILLE_GRILLE_JEU; i++)
            {
                GrillePieces.Add(new List<Image>());

                for (int j = 0; j < GrilleJeu.TAILLE_GRILLE_JEU; j++)
                {
                    position = new Coordonnee(i, j);

                    if (GrillePartie.EstCaseOccupee(position))
                    {
                        ImageAffichage = CreerAffichagePiece(GrillePartie.ObtenirPiece(position));

                        Grid.SetColumn(ImageAffichage, i);
                        Grid.SetRow(ImageAffichage, j);

                        grdPartie.Children.Add(ImageAffichage);

                        GrillePieces[i].Add(ImageAffichage);
                    }
                    else
                    {
                        GrillePieces[i].Add(null);
                    }
                }
            }
        }

        /// <summary>
        /// Retourne la source de l'image selon son type et son état de visibilité
        /// </summary>
        /// <param name="couleur"></param>
        /// <param name="pieceAAfficher"></param>
        /// <returns></returns>
        private string RetournerNomSourceImage(Couleur couleur, Piece pieceAAfficher)
        {
            string nomPiece;
            // On met le chemin relatif de l'image
            StringBuilder nomImageSource = new StringBuilder();
            if (couleur == Couleur.Rouge)
                nomImageSource.Append("Images/Rouge/");
            else
                nomImageSource.Append("Images/Bleu/");

            string nomImageSourceFinal;

            // On s'assure que le nom de la pièce soit en minuscule
            nomPiece = pieceAAfficher.GetType().ToString();
            nomPiece = nomPiece.Substring(9);
            nomPiece = nomPiece.ToLower();

            // Si la pièce dans le jeu n'a pas été révélé, on la cache.
            if (pieceAAfficher.EstRevele || pieceAAfficher.couleur == CouleurJoueur)
            {
                // On s'assure que le nom de la pièce soit en minuscule
                nomPiece = pieceAAfficher.GetType().ToString();
                nomPiece = nomPiece.Substring(9);
                nomPiece = nomPiece.ToLower();

            }
            else
            {
                nomPiece = "endos";
            }

            // On l'ajoute 
            nomImageSource.Append(nomPiece);

            if (couleur == Couleur.Rouge)
                nomImageSource.Append("R.png");
            else
                nomImageSource.Append("B.png");

            // On converti en string pour le retourner
            nomImageSourceFinal = nomImageSource.ToString();

            return nomImageSourceFinal;
        }
        /// <summary>
        /// Crée l'image dans la grille de jeu.
        /// </summary>
        /// <param name="pieceAffichage">La pièce avec laquelle on y crée une image dans la grille</param>
        /// <returns></returns>
        private Image CreerAffichagePiece(Piece pieceAffichage)
        {
            Image ImageAffichage = new Image();
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(RetournerNomSourceImage(pieceAffichage.couleur, pieceAffichage), UriKind.Relative);
            bitmap.EndInit();

            ImageAffichage.Stretch = Stretch.Fill;
            ImageAffichage.Source = bitmap;

            ImageAffichage.HorizontalAlignment = HorizontalAlignment.Center;
            ImageAffichage.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetZIndex(ImageAffichage, 2);

            return ImageAffichage;
        }

        // TODO: lorsqu'un pion adverse attaque et est éliminé, l'update se fait pas. Et si le pion adverse gagne, le update se fait donc mauvais.
        /// <summary>
        /// Activé par le clic droit de la souris, cette fonction vérifie la sélection active est présente dans la 
        /// grille de jeu. La sélection active représente la sélection du pion que le joueur veut déplacer.
        /// Une fois présente, la fonction vérifie si le joueur veut désélectionner son choix ou déplacer son pion sur une 
        /// autre case. Dans le cas d'un déplacement, la fonction appelle la fonction ExecuterCoup pour vérifier que ce coup est légal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResoudreSelectionCase(object sender, MouseButtonEventArgs e)
        {
            // Reçoit le rectangle sélectionné par la souris
            Rectangle caseSelectionnee = (Rectangle)sender;

            Coordonnee pointSelectionne = new Coordonnee(Grid.GetColumn(caseSelectionnee), Grid.GetRow(caseSelectionnee));
            Coordonnee pointActif;

            ReponseDeplacement reponse = new ReponseDeplacement();

            if (TourJeu == CouleurJoueur)
            {
                if (grdPartie.Children.Contains(SelectionActive))
                {
                    pointActif = new Coordonnee(Grid.GetColumn(SelectionActive), Grid.GetRow(SelectionActive));

                    // Permet de déselectionner notre choix si on change d'idée
                    if (pointSelectionne.EstEgal(pointActif))
                    {
                        grdPartie.Children.Remove(SelectionActive);
                    }
                    else
                    {
                        // On tente d'exécuter le coup
                        reponse = ExecuterCoup(pointActif, pointSelectionne);


                        if (reponse.DeplacementFait)
                        {
                            grdPartie.Children.Remove(SelectionActive);
                        }
                    }
                }
                else
                {
                    if (GrillePartie.EstCaseOccupee(pointSelectionne)
                       && GrillePartie.ObtenirCouleurPiece(pointSelectionne) == CouleurJoueur)
                    {
                        Grid.SetColumn(SelectionActive, (int)pointSelectionne.X);
                        Grid.SetRow(SelectionActive, (int)pointSelectionne.Y);

                        grdPartie.Children.Add(SelectionActive);
                    }
                }
            }



            if (reponse.FinPartie)
            {
                FinPartie();
            }
        }

        /// <summary>
        /// Si le coup est illégal, la fonction renvoi une réponse de déplacement négative pour laisser le joueur retenter un
        /// nouveau coup. Selon le résultat de la fonction ResoudreDeplacement, la fonction élimine de la GrillePartie et
        /// de la grid le pion vaincu.
        /// </summary>
        /// <param name="caseDepart">Coordonnée de la caseJeu sur laquelle le pion commence son déplacement</param>
        /// <param name="caseCible">Coordonnée de la  caseJeu sur laquelle le pion termine son déplacement</param>
        /// <returns></returns>
        public ReponseDeplacement ExecuterCoup(Coordonnee caseDepart, Coordonnee caseCible)
        {
            Thread executionIA = new Thread(LancerIA);


            ReponseDeplacement reponse = new ReponseDeplacement();

            Piece attaquant;
            Image affichageAttaquant;

            if (caseCible != caseDepart)
            {
                // Prendre les informations avant de faire le coup.
                attaquant = GrillePartie.ObtenirPiece(caseDepart);
                affichageAttaquant = GrillePieces[(int)caseDepart.X][(int)caseDepart.Y];

                reponse = GrillePartie.ResoudreDeplacement(caseDepart, caseCible);

                if (reponse.DeplacementFait)
                {

                    // Retrait de la pièce attaquante de sa position d'origine.
                    grdPartie.Children.Remove(affichageAttaquant);
                    GrillePieces[(int)caseDepart.X][(int)caseDepart.Y] = null;


                    if (reponse.PiecesEliminees.Count == 2)
                    {
                        // Retrait de la pièce attaquée.
                        grdPartie.Children.Remove(GrillePieces[(int)caseCible.X][(int)caseCible.Y]);
                        GrillePieces[(int)caseCible.X][(int)caseCible.Y] = null;

                        if(TourJeu == CouleurJoueur)
                            MettreAJourPionAdverseEliminees(reponse);
                    }
                    else if (reponse.PiecesEliminees.Count == 1 && reponse.PiecesEliminees[0] != attaquant
                            || reponse.PiecesEliminees.Count == 0)
                    {
                        // Remplacement de la pièce attaquée par la pièce attaquante.
                        grdPartie.Children.Remove(GrillePieces[(int)caseCible.X][(int)caseCible.Y]);
                        GrillePieces[(int)caseCible.X][(int)caseCible.Y] = null;


                        if (reponse.PiecesEliminees.Count == 1)
                        {
                            attaquant.EstRevele = true;
                            affichageAttaquant = CreerAffichagePiece(attaquant);
                            if(TourJeu == CouleurJoueur)
                                MettreAJourPionAdverseEliminees(reponse);
                        }

                        GrillePieces[(int)caseCible.X][(int)caseCible.Y] = affichageAttaquant;

                        Grid.SetColumn(affichageAttaquant, (int)caseCible.X);
                        Grid.SetRow(affichageAttaquant, (int)caseCible.Y);
                        grdPartie.Children.Add(affichageAttaquant);


                    }
                    else if (reponse.PiecesEliminees[0] == attaquant) // Lorsque la pièce attaquante perd.
                    {
                        Piece Occupant = GrillePartie.ObtenirPiece(caseCible);

                        if (TourJeu != CouleurJoueur)
                            MettreAJourPionAdverseEliminees(reponse);

                        // On révèle les informations de la pièce attaqué.
                        Occupant.EstRevele = true;
                        Image ImageOccupant = CreerAffichagePiece(Occupant);

                        grdPartie.Children.Remove(GrillePieces[(int)caseCible.X][(int)caseCible.Y]);
                        GrillePieces[(int)caseCible.X][(int)caseCible.Y] = null;

                        GrillePieces[(int)caseCible.X][(int)caseCible.Y] = ImageOccupant;

                        Grid.SetColumn(ImageOccupant, (int)caseCible.X);
                        Grid.SetRow(ImageOccupant, (int)caseCible.Y);
                        grdPartie.Children.Add(ImageOccupant);

                    }

                    ChangerTourJeu();

                    if (TourJeu == IA.CouleurIA)
                    {
                        executionIA.Start();
                    }

                }
            }
            else
            {
                reponse.DeplacementFait = false;
            }

            return reponse;
        }

        /// <summary>
        /// Détient une liste dans laquelle la fontion emmagasine les pièces de l'IA éliminées. De plus, il appelle
        /// la fonction du mainWindow pour mettre à jour l'affichage des pièces éliminées.
        /// </summary>
        /// <param name="reponse"></param>
        private void MettreAJourPionAdverseEliminees(ReponseDeplacement reponse)
        {
            List<Piece> LstPieceElimineeAjustement = new List<Piece>();
            StringBuilder StrBuilderLblNom = new StringBuilder();

            // On ajoute la piece éliminée de ce tour dans les pièces éliminées de la partie
            
            LstPiecesEliminees.Add(reponse.PiecesEliminees[0]);

            foreach(Piece PieceEliminee in LstPiecesEliminees)
            {
                if(PieceEliminee.GetType() == reponse.PiecesEliminees[0].GetType())
                {
                    LstPieceElimineeAjustement.Add(PieceEliminee);
                }
            }


            // On récupère le nom du type pour créer un nom de label
            string strNomLbl = LstPieceElimineeAjustement[0].GetType().ToString();
            StrBuilderLblNom.Append(strNomLbl.Substring(9));

            mainWindow.AjustementPieceEliminee(LstPieceElimineeAjustement, StrBuilderLblNom.ToString());



                
           
        }
        /// <summary>
        /// Fonction pour lancer l'IA pour qu'il joue son coup. 
        /// </summary>
      private void LancerIA()
      {
         // Pause d'une seconde, pour permettre à l'humain de mieux comprendre le déroulement.
         Thread.Sleep(1000);

         Dispatcher.Invoke(() =>
         {
            Notify();
         });
      }

        /// <summary>
        /// Permet de changer le tour de jouer entre le joueur et l'IA
        /// </summary>
      public void ChangerTourJeu()
      { 
         if (TourJeu == Couleur.Rouge)
         {
            TourJeu = Couleur.Bleu;
         }
         else
         {
            TourJeu = Couleur.Rouge;
         }
      }

        /// <summary>
        /// Lorsque le pion Drapeau d'un des 2 joueurs est éliminé, la fonction est appelé pour mettre fin 
        /// à la partie et dévoiler le gagnant.
        /// </summary>
        public void FinPartie()
        {
            MessageBoxResult resultat;
            resultat = MessageBox.Show("Le joueur " + TourJeu.ToString() + " a gagné! Voulez-vous recommencer la partie?"
                                       , "Fin de la partie"
                                      , MessageBoxButton.YesNo);
            if(resultat == MessageBoxResult.No)
            {
                Application.Current.Shutdown();
                //((MainWindow)App.Current.MainWindow).grdPrincipale.IsEnabled = false;
            }
            else
            {
                //((MainWindow)App.Current.MainWindow).grdPrincipale.IsEnabled = false;
                Application.Current.Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }

        
    }
}
