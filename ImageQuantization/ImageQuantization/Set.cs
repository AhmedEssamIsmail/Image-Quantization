using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    
    //class Set
    //{
    //    List<int> Rank, Parent;
    //    List<PalletteNode> Palette;
    //    int forests;
    //    /// <summary>
    //    /// initialize rank, parent, palette array by default values 
    //    /// initialize forests by the number of vertices
    //    /// </summary>
    //    /// <param name="Vertices">list of vertices</param>
    //    void initialize ( List<RGBPixel> Vertices) {
    //        int NumberOfVertices = Vertices.Count;
    //        Rank = new List<int>();
    //        Parent = new List<int>();
    //        Palette = new List<PalletteNode>();
    //        forests = NumberOfVertices;
    //        for (int i = 0; i < NumberOfVertices; i++)
    //        {
    //            Rank.Add(0);
    //            Parent.Add(i);
    //            Palette.Add(new PalletteNode(Vertices[i]));
    //        }
    //    }
        ///// <summary>
        ///// finds the element parent in the graph
        ///// </summary>
        ///// <param name="Child">child index</param>
        ///// <returns>Child's parent index</returns>
        //public int FindParent(int Child)
        //{
        //    if (Child == Parent[Child])
        //        return Child;
        //    return Parent[Child] = FindParent(Parent[Child]);
        //}
        ///// <summary>
        ///// merges two trees "Sets" together by their root elements  
        ///// </summary>
        ///// <param name="x">first vertex index</param>
        ///// <param name="y">second vertex index</param>
        //void Merge(int x, int y)
        //{
        //    x = FindParent(x);
        //    y = FindParent(y);
        //    if (Rank[x] > Rank[y])
        //    {
        //        int z = y;
        //        y = x;
        //        x = z;
        //    }
        //    Parent[x] = y;
        //    Palette[y].blue += Palette[x].blue;
        //    Palette[y].red += Palette[x].red;
        //    Palette[y].green += Palette[x].green;
        //    Palette[y].count += Palette[x].count;
        //    if (Rank[x] != Rank[y])
        //        Rank[y]++;
        //    forests--;
        //}
        ///// <summary>
        ///// Checks if the two elements have same parent
        ///// </summary>
        ///// <param name="x">first vertex index</param>
        ///// <param name="y">second vertex index</param>
        ///// <returns>true if the two vertices have same parent "in the same tree" otherwise returns false </returns>
        //bool SameParent(int x, int y)
        //{
        //    x = FindParent(x);
        //    y = FindParent(y);
        //    return x == y;
        //}
        /////// <summary>
        /////// constructs a minimum spanning tree
        /////// </summary>
        /////// <param name="Edges">list of edges contains the graphs edges</param>
        /////// <param name="Vertices">list of vertices contains the graphs vertices</param>
        /////// <returns>minimum spanning tree edges and weight as a pair</returns>
        //public Tuple<List<Edge>, double> Clustering(List<Edge> Edges, List<RGBPixel> Vertices , int NumberOfClusters)
        //{
        //    initialize(Vertices);
        //    int EdgesSize = Edges.Count;
        //    double MSTWeight = 0;
        //    List<Edge> E = new List<Edge>();
        //    for (int i = 0; i < EdgesSize&& NumberOfClusters > 0 ; i++ , NumberOfClusters -- )
        //    {
        //        if (!SameParent(Edges[i].Either().Number, Edges[i].Other(Edges[i].Either()).Number))
        //        {
        //            Merge(Edges[i].Either().Number, Edges[i].Other(Edges[i].Either()).Number);
        //        }
        //    }
        //    return new Tuple<List<Edge>, double>(E, MSTWeight);
        //}
        /// <summary>
        /// Divides the graph to clusters and generates the palette
        /// </summary>
        /// <param name="Edges">list of edges</param>
        /// <param name="Vertices"></param>
        /// <param name="NumberOfClusters"></param>
    //    public void GeneratePallette (List<Edge> Edges, List<RGBPixel> Vertices,int NumberOfClusters)
    //    {
    //        initialize(Vertices);
    //        int PalletteSize = Palette.Count;
    //        int EdgesSize = Edges.Count;
    //        List<Edge> E = new List<Edge>();
    //        for (int i = 0; i < EdgesSize; i++)
    //        {
    //            if (forests == NumberOfClusters)
    //                break;
    //            if (!SameParent(Edges[i].Either().Number, Edges[i].Other(Edges[i].Either()).Number))
    //            {
    //                E.Add(Edges[i]);
    //                Merge(Edges[i].Either().Number, Edges[i].Other(Edges[i].Either()).Number);
    //            }
    //        }
    //        for (int i = 0; i < PalletteSize; i++)
    //        {
    //            if (Parent[i]==i)
    //            {
    //                Palette[i].blue /= Palette[i].count;
    //                Palette[i].red /= Palette[i].count;
    //                Palette[i].red /= Palette[i].count;
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// get a color from the color palette
    //    /// </summary>
    //    /// <param name="Vertex"></param>
    //    /// <returns>RGB color</returns>
    //    public PalletteNode GetColour(RGBPixel Vertex)
    //    {
    //        return Palette[Parent[Vertex.Number]];
    //    }
    //}
}
