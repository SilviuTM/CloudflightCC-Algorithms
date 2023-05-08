string? input = Console.ReadLine();
if (input == null)
{
    Console.WriteLine("String null! Abort.");
    return;
}
else
{
    int maxBid, price, BUYNOW;
    string maxBidder;

    string[] data = input.Split(',');

    price = int.Parse(data[0]);
    BUYNOW = int.Parse(data[1]);
    maxBidder = data[2];
    maxBid = int.Parse(data[3]);

    Console.Write("-,{0},{1},{2}", price, maxBidder, price);

    for (int i = 4; i < data.GetLength(0); i += 2)
    {
        int curBid = int.Parse(data[i+1]);
        string curBidder = data[i];
        bool differentBidder = maxBidder != curBidder;

        if (curBid < maxBid)
        {
            if (curBid >= price) if (differentBidder) price = curBid + 1;
        }
        else if (curBid == maxBid) 
        {
            if (differentBidder) price = curBid;
        }
        else
        {
            if (differentBidder)
                price = maxBid + 1;

            maxBid = curBid;
            maxBidder = curBidder;
        }

        if (differentBidder)
            if (price < BUYNOW || BUYNOW <= 0)
                Console.Write(",{0},{1}", maxBidder, price);
            else
            {
                Console.Write(",{0},{1}", maxBidder, BUYNOW);
                return;
            }
    }

    //Console.WriteLine("{0},{1}", maxBidder, price);
}