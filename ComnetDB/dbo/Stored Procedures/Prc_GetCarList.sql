CREATE PROCEDURE [dbo].[Prc_GetCarList]
(				
	@pPageNumber INT = 1,
	@pRowsPerPage INT = 2,	
	@pOrderBy VARCHAR(MAX),
	@pKeyWord VARCHAR(MAX) = NULL,
	@pTotalCount INT output
)
AS
BEGIN

	DROP TABLE IF EXISTS #TEMP_TableList
	CREATE TABLE  #TEMP_TableList (
		RowNumber INT,
		UnqGUID UNIQUEIDENTIFIER,
		[Name] NVARCHAR(100),
		[Code] NVARCHAR(100),
		[URL]  NVARCHAR(MAX)
	)
	
	INSERT INTO #TEMP_TableList(RowNumber, UnqGUID, [Name], [Code], [URL])
	SELECT
		ROW_NUMBER() OVER(ORDER BY DateOfManufactring, COALESCE(SortOrder, 0), CreatedDate DESC) AS RowNumber,
		UnqGUID,
		[Name],
		[Code],
		I.[URL]
	FROM CM_Cars C
	OUTER APPLY (
		SELECT TOP 1 URL FROM CM_CarImages WHERE IsDeleted = 0 AND CarID = C.UnqGUID ORDER BY [Order]
	) I
	WHERE IsDeleted = 0
	AND ((@pKeyWord IS NULL OR @pKeyWord = '')
		OR CHARINDEX('' + @pKeyWord + '', [Name] , 1) > 0
		OR CHARINDEX('' + @pKeyWord + '', [Code], 1) > 0)
	
	SELECT
		UnqGUID,
		[Name],
		[Code],
		[Url]
	FROM #TEMP_TableList
	WHERE @pPageNumber != 0 AND @pRowsPerPage != 0
		AND RowNumber BETWEEN CAST(((@pPageNumber - 1) * @pRowsPerPage) + 1 AS VARCHAR) AND CAST(@pRowsPerPage * @pPageNumber AS VARCHAR)

	SELECT @pTotalCount = COUNT(*) FROM #TEMP_TableList

	DROP TABLE IF EXISTS #TEMP_TableList
		
END