DECLARE @KcRoomUsageTypeId int = 6

UPDATE [dbo].[Rooms]
SET [Room_Usage_Type_ID] = @KcRoomUsageTypeId
WHERE Room_ID IN (1957,1956,1954,1952,1950,1951,1953,1955,1939,1940,1946,1947,1948,1949,1958,1960,1941,1942,1943,1944,1945,1889,1888,1887,1885,1884,1883,1890,1891,1892,1893,1894,1895,1896,1897,1898,1899,1900,1901,1902,1903,1904,1905,1908,1909,1910,1819,1820,1822,1823,1824,1825,1821,1826,1870,1864,1873,1818,1829,1861,1871,1862,1832,1833,1834,1835,1836,1837,1838,1839,1840,1841,1842,1843,1844,1845,1846,1847,1852,1850,1853,1854,1855,1856,1814,2006,2007,2012,2009,2010,1984,1978,1976,1974,1975,1982,1980,1972,1989,1977,1979,1981)
