using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    /// <summary>
    /// a class that represent a graph with its vertices and edges and its minimum spaning tree
    /// </summary>
    public class Graph
    {
        public List<RGBPixel> Vertices;
        public List<Edge> Edges;
        Dictionary<string, RGBPixel> VerticesExist;
        public Dictionary<int, Tuple<int, double>> MSTVertices;
        public double MSTWeight;
        int Size;
        public Edge [] Edges_Arr ;

        public Graph()
        {
            Vertices = new List<RGBPixel>();
            VerticesExist = new Dictionary<string, RGBPixel>();
            Edges = new List<Edge>();
            Size = 0;
        }
        /// <summary>
        /// Iterates on the 2D array (ImageMatrix) and stores the distinct colors in a list of vertices
        /// </summary>
        /// <param name="ImageMatrix"></param>
        public Graph(RGBPixel[,] ImageMatrix)
        {
            Vertices = new List<RGBPixel>();
            VerticesExist = new Dictionary<string, RGBPixel>();
            MSTVertices = new Dictionary<int, Tuple<int, double>>();
            int Height = ImageOperations.GetHeight(ImageMatrix);
            int Width = ImageOperations.GetWidth(ImageMatrix);
            Size = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    ImageMatrix[i, j].ConvertRGBToString();
                    if (VerticesExist.ContainsKey(ImageMatrix[i, j].RGBString))
                        continue;
                    AddVertex(ImageMatrix[i, j]);
                }
            }
        }
        /// <summary>
        /// Adds a vertex to the graph
        /// </summary>
        /// <param name="Vertex">RGBpixel contains the vertex</param>
        void AddVertex(RGBPixel Vertex)
        {
            Size++;
            Vertex.Number = Size - 1;
            Vertices.Add(Vertex);
            VerticesExist[Vertex.RGBString] = new RGBPixel();
            MSTVertices[Vertex.Number] = new Tuple<int, double>(0, Int32.MaxValue);
        }
        public long DetectTheNumberOfClusters()
        {
            int K = 1;
            double X1= double.MaxValue, X2;
            List<double> Eweight = new List<double>();
            int MaximumIndex = 0, NumberOfEdges = Size - 1;
            MessageBox.Show(NumberOfEdges.ToString());
            double Mean, MeanBefore = 0, StandDev = 0, StandDevNext = 0, MaximumValue = -1;
            for (int i = 0; i < Size - 1 ; i++)
            {
                MeanBefore += Edges[i].Weight;
                Eweight.Add(Edges[i].Weight);
            }
            Mean = MeanBefore / NumberOfEdges;
            for (int i = 0; i < Size - 1; i++)
            {
                double TEMP = Eweight[i] - Mean;
                TEMP = (TEMP < 0) ? TEMP *= -1 : TEMP;
                TEMP *= TEMP;
                StandDev += TEMP;
                if (TEMP > MaximumValue)
                {
                    MaximumValue = TEMP;
                    MaximumIndex = i;
                }
            }
            StandDev = Math.Sqrt(StandDev / (NumberOfEdges-1));
            while (true)
            {
                NumberOfEdges--;
                MeanBefore -= Eweight[MaximumIndex];
                Mean = MeanBefore / (NumberOfEdges);
                Eweight[MaximumIndex] = -1;
                StandDevNext = 0;
                MaximumValue = -1;
                for (int i = 0; i < Size - 1; i++)
                {
                    if (Eweight[i] == -1)
                        continue;
                    double TEMP = (Eweight[i] - Mean);      //Calculates the Variance
                    TEMP = (TEMP < 0) ? TEMP *= -1 :TEMP ;  //Absolutes the value
                    TEMP *= TEMP;                           //Pow (Temp,2) 
                    StandDevNext += TEMP;
                    if (TEMP > MaximumValue)
                    {
                        MaximumValue = TEMP;
                        MaximumIndex = i;
                    }
                }
                StandDevNext = Math.Sqrt(StandDevNext / (NumberOfEdges-1));
                X2 = StandDev - StandDevNext;
                if (Math.Abs (X1 - X2)< 0.0001)
                {
                    return K;
                }
                K++;
                if (NumberOfEdges==2)
                { return K; }
                X1 = X2;
                StandDev = StandDevNext;
            }
        }
        public void ConstructMST()
        {
            Edges = new List<Edge>();
            int DisconnectedVertices = Size - 1;
            RGBPixel Vertex = Vertices[0];
            double MinimumWeight, Weight;
            int MinimumValue = 0;
            MSTWeight = 0;
            MSTVertices[0] = new Tuple<int, double>(Vertex.Number, -1);
            Edges_Arr = new Edge[100000];
            int Edges_Size = 0; 
            while (DisconnectedVertices != 0)
            {
                MinimumWeight = int.MaxValue;
                for (int i = 0; i < Size; i++)
                {
                    if (MSTVertices[i].Item2 == -1)
                    {
                        continue;
                    }
                    Weight = Math.Sqrt(((Vertex.red - Vertices[i].red) * (Vertex.red - Vertices[i].red))
                            + ((Vertex.green - Vertices[i].green) * (Vertex.green - Vertices[i].green)) +
                             ((Vertex.blue - Vertices[i].blue) * (Vertex.blue - Vertices[i].blue)));
                    if (Weight < MSTVertices[i].Item2)
                    {
                        MSTVertices[i] = new Tuple<int, double>(Vertex.Number, Weight);
                    }
                    else
                    {
                        Weight = MSTVertices[i].Item2;
                    }
                    if (Weight < MinimumWeight)
                    {
                        MinimumWeight = Weight;
                        MinimumValue = Vertices[i].Number;
                    }
                }
                Edges.Add(new Edge(Vertices[MinimumValue], Vertices[MSTVertices[MinimumValue].Item1], MSTVertices[MinimumValue].Item2));
                Edges_Arr[Edges_Size] = new Edge(Vertices[MinimumValue], Vertices[MSTVertices[MinimumValue].Item1], MSTVertices[MinimumValue].Item2);
                Edges_Size++;
                MSTWeight += MinimumWeight;
                Vertex = Vertices[MinimumValue];
                MSTVertices[MinimumValue] = new Tuple<int, double>(Vertex.Number, -1);
                DisconnectedVertices--;
            }
            MessageBox.Show(MSTWeight.ToString());
            //QuickSort(0, Edges.Count - 1, Edges, Edges.Count);
            Edges = Merge_Sort(Edges_Arr, Edges_Size).ToList();
            //Bubble(Edges, Edges.Count);
            //Edges.Sort((Edge E1, Edge E2) =>  E1.Weight.CompareTo(E2.Weight));
            return;
        }
        public Edge[] Merge_Sort (Edge [] E,int Size)
        {
            if (Size == 1) return E;
            int MidPoint = Size / 2;
            Edge[] Left = new Edge[MidPoint];
            Edge[] Right = new Edge[Size - MidPoint];
            for (int i = 0; i < MidPoint; i++)
            {
                Left[i] = E[i];
            }
            for (int i = MidPoint; i < Size; i++)
            {
                Right[i - MidPoint] = E[i];
            }
            Left = Merge_Sort(Left, MidPoint);
            Right = Merge_Sort(Right, Size - MidPoint);
            return Combine(Left, Right,Size-MidPoint, MidPoint);
        }
        public Edge[] Combine (Edge[]Right , Edge[]Left , int LeftSize,int RightSize)
        { 
            Edge[] Sorted = new Edge[RightSize + LeftSize];
            for (int k = 0,i=0,j=0; k < LeftSize+RightSize; k++)
            {
                if (i>=LeftSize){
                    Sorted[k] = Right[j];
                    j++;
                }
                else if (j>=RightSize){
                    Sorted[k] = Left[i];
                    i++;
                }
                else if (Left[i].Weight <= Right[j].Weight){
                    Sorted[k] = Left[i];
                    i++;
                }
                else{
                    Sorted[k] = Right[j];
                    j++;
                }
            }
            return Sorted; 
        }
        public void Bubble(List<Edge> E, long N)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    if (i == j)
                        continue;
                    
                    if (E[i].Weight >= E[j].Weight)
                    {
                        Swap(i, j, E);
                    }
                }
            }
        }
        /// <summary>
        /// Sorts the edges in an ascending order by its weight and pass it by refrence 
        /// </summary>
        /// <param name="startIndex">the index of the first element of the subset of edges</param>
        /// <param name="finalIndex">the index of the last element of the subset of edges</param>
        /// <param name="E">the List of edges</param>
        /// <param name="N">Size of the Edges List</param>
        public void QuickSort(int startIndex, int finalIndex, List<Edge> E, long N)
        {
            if (startIndex >= finalIndex) return;
            int i = startIndex, j = finalIndex;
            while (i <= j)
            {
                while (i < N && E[startIndex].Weight >= E[i].Weight) i++;
                while (j > -1 && E[startIndex].Weight < E[j].Weight) j--;
                if (i <= j)
                {
                    Swap(i, j, E);
                }
            }
            Swap(startIndex, j, E);
            QuickSort(startIndex, j, E, j+1);
            QuickSort(i, finalIndex, E, finalIndex+1);
        }
        /// <summary>
        /// swaps two elements 
        /// </summary>
        /// <param name="i">the index of the first element</param>
        /// <param name="j">the index of the second element</param>
        /// <param name="collection">list of elements</param>
        public static void Swap(int i, int j, List<Edge> collection)
        {
            Edge tmp = collection[i];
            collection[i] = collection[j];
            collection[j] = tmp;
        }
        /// <summary>
        /// Constructs the edges between each of the vertices
        /// </summary>
        public void ConstructEdges()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    Edges.Add(new Edge(Vertices[i], Vertices[j]));
                }
            }
        }



        /// <summary>
        /// holds a palette colour with its red ,green and blue bytes 
        /// holds the number of vertices in that tree : count
        /// </summary>
        public class PalletteNode
        {
            public double red, green, blue;
            public int count;
            public RGBPixel Vertex;
            public PalletteNode(RGBPixel V)
            {
                red = V.red;
                green = V.green;
                blue = V.blue;
                count = 1;
            }
            public PalletteNode()
            {
                red = 0;
                green = 0;
                blue = 0;
                count = 0;
            }
            public void CalculatePallette()
            {
                Vertex = new RGBPixel();
                Vertex.red = (byte)(this.red / count);
                Vertex.green = (byte)(this.green / count);
                Vertex.blue = (byte)(this.blue / count);
            }
        }
        ///// <summary>
        ///// constructs a minimum spanning tree
        ///// </summary>
        ///// <param name="Edges">list of edges contains the graphs edges</param>
        ///// <param name="Vertices">list of vertices contains the graphs vertices</param>
        ///// <returns>minimum spanning tree edges and weight as a pair</returns>
        public void Clustering( int NumberOfClusters)
        {
            List<int> Rank, Parent;
            List<PalletteNode> Palette;
            Rank = new List<int>();
            Parent = new List<int>();
            Palette = new List<PalletteNode>();
            int forests=Size;
            initialize(Vertices,Rank,Parent,Palette);
            for (int i = 0; i < Size-1 && NumberOfClusters != forests; i++)
            {
                    Merge(Edges[i].Either().Number, Edges[i].Other(Edges[i].Either()).Number, Rank, Parent, Palette);
                    forests--;
            }
            for (int i = 0; i < Size; i++)
            {
                int a = FindParent(i,Parent);
                if (a==i)
                {
                    Palette[i].CalculatePallette();
                }
            }
            GeneratePallette(Parent,Palette);
            return;
        }
        public void K_MeanClustring (int NumberOfClusters)
        {
            List<PalletteNode> Clusters = new List<PalletteNode>();
            List<PalletteNode> NewClusters = new List<PalletteNode>();
            List<int> Parent = new List<int>();
            for (int i = 0; i < Size; i++)
            {
                Parent.Add(new int());
                if (i >= NumberOfClusters)
                    continue;
                Clusters.Add(new PalletteNode());
                NewClusters.Add(new PalletteNode(Vertices[i]));
            }
            while (!EqualClusters(Clusters, NewClusters, NumberOfClusters))
            {
                InitializeNewClusters(NewClusters,Clusters,NumberOfClusters);
                for (int i = 0; i < Size; i++)
                {
                    double MinimumDistance = double.MaxValue;
                    for (int j = 0; j < NumberOfClusters; j++)
                    {
                        double Temp = ((Clusters[j].red - Vertices[i].red) * (Clusters[j].red - Vertices[i].red))
                                    + ((Clusters[j].green - Vertices[i].green) * (Clusters[j].green - Vertices[i].green))
                                    + ((Clusters[j].blue - Vertices[i].blue) * (Clusters[j].blue - Vertices[i].blue));
                        if (Temp < MinimumDistance)
                        {
                            MinimumDistance = Temp;
                            Parent[i] = j;
                        }
                    }
                    NewClusters[Parent[i]].red += Vertices[i].red;
                    NewClusters[Parent[i]].green += Vertices[i].green;
                    NewClusters[Parent[i]].blue += Vertices[i].blue;
                    NewClusters[Parent[i]].count++;
                }
                for (int i = 0; i < NumberOfClusters; i++)
                {
                    if (NewClusters[i].count == 0)
                        continue;
                    NewClusters[i].red /= NewClusters[i].count;
                    NewClusters[i].green /= NewClusters[i].count;
                    NewClusters[i].blue /= NewClusters[i].count;
                }
            }
            GeneratePalletteKMean(Clusters, Parent,NumberOfClusters);
        }
        public bool EqualClusters (List<PalletteNode> P1 , List <PalletteNode> P2 ,int K)
        {
            for (int i = 0; i < K; i++)
            {
                if (P1[i].red != P2[i].red || P1[i].green != P2[i].green || P1[i].blue != P2[i].blue||P1[i].count != P2[i].count)
                    return false;
            }
            return true;
        }
        public void InitializeNewClusters (List <PalletteNode> P1,List<PalletteNode> P2,int K)
        {
            for (int i = 0; i < K; i++)
            {
                P2[i]=P1[i];
                P1[i] = new PalletteNode();
            }
        }
        public void GeneratePalletteKMean(List<PalletteNode> Clusters ,List<int> Parent, int K)
        {
            for (int i = 0; i < K; i++)
            {
                Clusters[i].Vertex = new RGBPixel();
                Clusters[i].Vertex.red = (byte)(Clusters[i].red );
                Clusters[i].Vertex.green = (byte)(Clusters[i].green );
                Clusters[i].Vertex.blue = (byte)(Clusters[i].blue );
            }
            for (int i = 0; i < Size; i++)
            {
                VerticesExist[Vertices[i].RGBString] = Clusters[Parent[i]].Vertex;
            }
        }
        /// <summary>
        /// Checks if the two elements have same parent
        /// </summary>
        /// <param name="x">first vertex index</param>
        /// <param name="y">second vertex index</param>
        /// <returns>true if the two vertices have same parent "in the same tree" otherwise returns false </returns>
        bool SameParent(int x, int y, List<int> Parent)
        {
            x = FindParent(x, Parent);
            y = FindParent(y, Parent);
            return x == y;
        }
        /// <summary>
        /// merges two trees "Sets" together by their root elements  
        /// </summary>
        /// <param name="x">first vertex index</param>
        /// <param name="y">second vertex index</param>
        void Merge(int x, int y, List<int> Rank, List<int> Parent, List<PalletteNode> Palette)
        {
            x = FindParent(x, Parent);
            y = FindParent(y, Parent);
            if (Rank[x] > Rank[y])
            {
                int z = y;
                y = x;
                x = z;
            }
            Parent[x] = y;
            Palette[y].blue += Palette[x].blue;
            Palette[y].red += Palette[x].red;
            Palette[y].green += Palette[x].green;
            Palette[y].count += Palette[x].count;
            if (Rank[x] == Rank[y])
                Rank[y]++;
        }
        /// <summary>
        /// finds the element parent in the graph
        /// </summary>
        /// <param name="Child">child index</param>
        /// <returns>Child's parent index</returns>
        public int FindParent(int Child,  List<int> Parent)
        {
            if (Child == Parent[Child])
                return Child;
            return Parent[Child] = FindParent(Parent[Child], Parent);
        }
        /// <summary>
        /// initialize rank, parent, palette array by default values 
        /// initialize forests by the number of vertices
        /// </summary>
        /// <param name="Vertices">list of vertices</param>
        void initialize(List<RGBPixel> Vertices, List<int> Rank, List<int> Parent,List <PalletteNode> Palette)
        {
            int NumberOfVertices =Size;
            for (int i = 0; i < NumberOfVertices; i++)
            {
                Rank.Add(0);
                Parent.Add(i);
                Palette.Add(new PalletteNode(Vertices[i]));
            }
        }
        void GeneratePallette (List<int> Parent,List<PalletteNode> Pallette)
        {
            for (int i = 0; i < Size; i++)
            {
                int parent = FindParent(i, Parent);
                VerticesExist[Vertices[i].RGBString] = Pallette[parent].Vertex;
            }
        }
        public void ConstructTheNewImage(RGBPixel[,] ImageMatrix)
        {
            int Height = ImageOperations.GetHeight(ImageMatrix);
            int Width = ImageOperations.GetWidth(ImageMatrix);
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    ImageMatrix[i, j] = VerticesExist[ImageMatrix[i, j].RGBString];
                }
            }
        }
    }
}