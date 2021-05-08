namespace HW5P2Lib

module HW5P2 =

    open FSharp.Data

    type Article(a,b,c,d,e,f,g,h,i,j,k,l,m) =
        member this.news_id = a
        member this.url = b
        member this.publisher = c
        member this.publish_date = d
        member this.author = e
        member this.title = f
        member this.image = g
        member this.body_text = h
        member this.news_guard_score = i
        member this.mbfc_level = j
        member this.political_bias = k
        member this.country = l
        member this.reliablity = m

    //
    // doubleOrNothing
    //
    // Given a string containing a double numeric value
    // or being an empty string
    // returns the double equivalent, with the empty string
    // treated as the value 0.0
    //
    let doubleOrNothing s = 
        match s with
        | "" -> 0.0
        | x -> double x

    //
    // stringToInteger
    //
    // Given a string containing an integer value
    // or being an empty string
    // returns the integer equivalent, with the empty string
    // treated as the value 0
    //
    let stringToInteger s = 
        match s with
        | "" -> 0
        | x -> int x


    let charListToString L =
        let sb = System.Text.StringBuilder()
        L |> List.iter (fun c -> ignore (sb.Append (c:char)))
        sb.ToString()

    let stringToCharList s =
        Seq.toList s
    
    let rec contains x L =
        match L with
        | [] -> false
        | hd::tl when x=hd -> true
        | hd::tl -> contains x tl

    let rec distinct L =
        let rec _distinct L1 L2 =
            match L1 with
            | [] -> List.rev L2
            | hd::tl -> match contains hd L2 with
                        | true -> _distinct tl L2
                        | false -> _distinct tl (hd::L2)
        _distinct L []


    //// Implement functions for 10 questions 


    // Given the news id and a list of articles,
    // return the correpsonding article
    // the output of this function can be used as
    // the function input for first three questions 
    let rec getArticle index (collection: Article list) =
        match collection with
        | [] -> failwith "index not found"
        | hd::tl when hd.news_id = index -> hd
        | _::tl -> getArticle index tl


    // 1. Given a news id, write a function to return
    //    its corresponding news title.
    let getTitle index allData =
        let article = getArticle index allData
        article.title


    //// 2. Given a news id, write a function to return 
    ////    the length (number of words) of its corresponding news body text.
    ////    Words are separated by space or line separator "\n".
    let rec splitOnSpace prev count L =
        match prev with
        | ' ' -> match L with
                 | [] -> count
                 | ' '::tl -> splitOnSpace prev count tl
                 | '\n'::tl -> splitOnSpace prev count tl
                 | hd::tl -> splitOnSpace hd (count+1) tl
        | '\n' -> match L with
                  | [] -> count
                  | ' '::tl -> splitOnSpace prev count tl
                  | '\n'::tl -> splitOnSpace prev count tl
                  | hd::tl -> splitOnSpace hd (count+1) tl
        | _ -> match L with
                 | [] -> count
                 | hd::tl -> splitOnSpace hd count tl

    let wordCount index allData =
        let A = getArticle index allData
        let text = A.body_text
        let charList = stringToCharList text
        let numWords = splitOnSpace ' ' 0 charList
        numWords



    //// 3. Given a news id , write a function to return
    ////    its corresponding publish month as a string.
    ////    For example, return “January” for 2020-01-21, return “June” for 2020-06-23.
    let rec findMonthMid L built onMonth =
        match L with
        | [] -> []
        | '-'::tl when onMonth = false -> findMonthMid tl built true
        | '-'::tl when onMonth = true -> List.rev built
        | hd::tl when onMonth = true -> findMonthMid tl (hd::built) true
        | hd::tl -> findMonthMid tl built false

    let rec findMonth L built onMonth =
        match L with
        | [] -> []
        | '/'::tl when onMonth = false -> findMonth tl built true
        | '/'::tl when onMonth = true -> List.rev built
        | hd::tl when onMonth = true -> findMonth tl (hd::built) true
        | hd::tl -> findMonth tl built false

    let getMonthStringFromNumber monthN =
    // since imploding the list will return a string, we should match with strings
        match monthN with
        | "01" | "1" -> "January"
        | "02" | "2" -> "February"
        | "03" | "3" -> "March"
        | "04" | "4" -> "April"
        | "05" | "5" -> "May"
        | "06" | "6" -> "June"
        | "07" | "7" -> "July"
        | "08" | "8" -> "August"
        | "09" | "9" -> "September"
        | "10" -> "October"
        | "11" -> "November"
        | "12" -> "December"
        | _ -> "Unknown"

    let getMonthNameFromArticle (A: Article) =
        let date = A.publish_date
        let datelist = stringToCharList date
        let monthInList = findMonth datelist [] true //false if middle
        let monthnumber = charListToString monthInList
        let actualmonth = getMonthStringFromNumber monthnumber
        actualmonth

    let getMonthName index allData =
        let A = getArticle index allData
        getMonthNameFromArticle A

    let getMonth (A: Article) =
        let actualMonth = getMonthNameFromArticle A
        actualMonth

    //// 4. Write a function to return a list of unique news publishers.
    ////    Each publisher is represented by its original name in the string format.
    ////    The order of the publishers in the output list is the same as the order in the CSV data.
    let publishers allData =
        allData
        |> List.map (fun (x: Article) -> x.publisher)
        |> distinct //List.distinct
        //|> List.sort


    //// 5. Write a function to return a list of unique countries.
    ////    Each country is represented by its original name in the string format.
    ////    The order of the countriesin the output list is the same as the order in the CSV data.
    let countries allData =
        allData
        |> List.map (fun (x: Article) -> x.country)
        |> distinct //List.distinct
        //|> List.sort



    //// 6. Write a function to return the average news_guard_score among all news articles.
    let avgNewsguardscoreForArticles allData =
        List.averageBy (fun (x:Article) -> x.news_guard_score) allData



    //// 7. Write a function to return a list containing the amount of news for each month,
    ////    from January to December. Each month and its corresponding news amount is represented by a tuple.
    ////    You need to print this information as a histogramin the console.
    ////    The histogram can be represented by stars (*).
    ////    Each star (*) should represents 1% of overall amount of data.
    ////    More instructions to print the histogram can be found in the project description.
    let _numberOfArticles allData monthName =
        let count = allData
                    |> List.filter (fun (x: Article) -> (getMonthNameFromArticle x) = monthName)
                    |> List.length
        (monthName,count)

    let numberOfArticles allData monthNames = 
        List.map (_numberOfArticles allData) monthNames

    let numberOfArticlesEachMonth allData =
        let months = ["January"; "February"; "March"; "April"; "May"; "June"; "July"; "August"; "September"; "October"; "November"; "December"]
        numberOfArticles allData months




    //// 8. Write a function to return a list of unique publishers
    ////    and the percentage of news which are marked as reliable (1) published by each publisher.
    ////    Each publisher and its reliable news percentage are represented by a tuple, e.g., (“CNN”, 65.233).
    ////    The order of the tuples are the same as the order of publishers in the CSV data.
    let _reliablepercentages allData publisherN =
        let totalbypublisher = allData
                               |> List.filter (fun (x: Article) -> x.publisher = publisherN)
        let totalcount = List.length totalbypublisher
        let reliablecount = totalbypublisher
                            |> List.filter (fun (x: Article) -> x.reliablity = 1)
                            |> List.length
        let count = (float(reliablecount) / float(totalcount)) * 100.0
        (publisherN,count)

    let reliableArticlePercentEachPublisher allData =
        let listPublishers = publishers allData
        List.map (_reliablepercentages allData) listPublishers




    //// 9. Write a function to return a list of unique counties and their average news_guard_score.
    ////    The output is a list of tuples, e.g., [(“USA”, 92.5), (“Russia”, 35.222)].
    ////    The order of the tuples are the same as the order of countries in the CSV data.
    let _averageguardscore allData countryN =
        let totalbycountry = allData
                             |> List.filter (fun (x: Article) -> x.publisher = countryN)
        let totalcount = List.length totalbycountry
        let (guardscore : float) = totalbycountry
                                    |> List.sumBy (fun (x: Article) -> x.news_guard_score)

        let count = guardscore / (float totalcount)
        (countryN,count)

    let averageguardscore allData =
        let listcountries = countries allData
        List.map (_averageguardscore allData) listcountries

    let averageguardscoreoverall allData =
        let guardscore = List.sumBy (fun (x: Article) -> x.news_guard_score) allData
        let totalcount = List.length allData
        float(guardscore) / float(totalcount)

    let rec avgNewsguardscorePerCountry allData countryNames =
        match countryNames with
        | [] -> []
        | hd::tl -> let filteredData = List.filter (fun (x:Article) -> x.country=hd) allData
                    let guardscore = List.averageBy (fun (x: Article) -> x.news_guard_score) filteredData
                    (hd,guardscore)::(avgNewsguardscorePerCountry allData tl)

    let rec avgNewsguardscoreEachCountry allData countryNames =
        avgNewsguardscorePerCountry allData (countries allData)



    //// 10. The political_bias column has multiple values, write a function to categorize them and 
    ////     return a list of tuples with each category and its average news_guard_score.
    ////     The order of the tuples are the same as the order of political_bias in the CSV data.
    ////     Similar to Question 7, you need to print the output as a histogram. You can reuse the code from Question 7.
    let getUniquePoliticalBias allData =
        allData
        |> List.map (fun (x: Article) -> x.political_bias)
        |> distinct //List.distinct
        //|> List.sort

    let _politicalbias allData category =
        let totalbycategory = allData
                              |> List.filter (fun (x: Article) -> x.political_bias = category)
        let totalcount = List.length totalbycategory
        let (guardscore : float) = totalbycategory
                                    |> List.sumBy (fun (x: Article) -> x.news_guard_score)

        let count = guardscore / float(totalcount)

        (category,count)

    let avgNewsguardscoreEachBias allData =
        let categories = getUniquePoliticalBias allData
        List.map (_politicalbias allData) categories



    let rec printAllItemsInList L =
        List.map (string) L

    let rec printStringsAndIntegers L =
        List.map (fun (s,i) -> sprintf "%s: %d\n" s i) L

    let rec printNamesAndFloats L =
        List.map (fun (s,f) -> sprintf "%s: %.3f\n" s f) L

    let rec printNamesAndPercentages L =
        List.map (fun (s,f) -> sprintf "%s: %.3f %%\n" s f) L

    let rec printStars n s =
        match n with
        | 0 -> s
        | 1 -> s + "*"
        | _ -> let newS = s + "*"
               printStars (n-1) newS

    let rec buildHistogram L numberPer stringSoFar =
        match L with
        | [] -> stringSoFar
        | (category,number)::tl -> let prefix = sprintf " %15s : " category
                                   //let numStars = number // or some function of number
                                   let numStars = (number*100)/numberPer
                                   let body = printStars numStars ""
                                   let body2 = sprintf "%d/n" number
                                   let newStringSoFar = stringSoFar + prefix + body + body2
                                   buildHistogram tl numberPer newStringSoFar


    let rec buildHistogramFloat L stringSoFar =
        match L with
        | [] -> stringSoFar
        | (category,(number:double))::tl -> let prefix = sprintf " %15s : " category
                                            let numStars = (int number)/5
                                            let body = printStars numStars ""
                                            let body2 = sprintf "%.3f/n" number
                                            let newStringSoFar = stringSoFar + prefix + body + body2
                                            buildHistogramFloat tl newStringSoFar
    //////////////////////////////

    let readfile (filename: string) = 
        let contents = CsvFile.Load(filename)
        
        // List comprehension magic to build a list of Article objects
        // See definition of Article class at the top (type Article)
        let data = [for x in contents.Rows
                     do
                        Article(stringToInteger (x.GetColumn 0),
                                x.GetColumn 1,
                                x.GetColumn 2,
                                x.GetColumn 3,
                                x.GetColumn 4,
                                x.GetColumn 5,
                                x.GetColumn 6,
                                x.GetColumn 7,
                                doubleOrNothing (x.GetColumn 8),
                                x.GetColumn 9,
                                x.GetColumn 10,
                                x.GetColumn 11,
                                stringToInteger (x.GetColumn 12))]

        data
