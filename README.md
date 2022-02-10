# Conway's Game of Life
## Build Instructions
1. Open the 'Life' folder
2. Open the 'Life.sln' file with Microsoft Visual Studio
3. If only running on the default settings: then just start the program
4. If wanting to change the settings: then refer to the Usage section on the possible
	options that can be changed. Then start the program.
5. Press Spacebar to start the simulation
...

## Usage 
1. Navigate to the 'life.dll' file in the cmd.
	
	1-1. To make things easier: path/to/folder/Life/Life/bin/Debug/netcoreapp3.1
2. Type 'dotnet life.dll' on the cmd once you are in the correct location.
3. Press Spacebar to start the simulation.
4. If wanting to change the settings, type the options and parameters after dotnet life.dll.
5. The options available are:
	
	5-1. Dimensions: --dimensions {rows} {columns} : Changes the rows and columns of the grid
	
		5-1-1. The default values for the grid is 16x16.
		5-1-2. The dimensions must be between 4 and 48 (inclusive).
	
	5-2. Periodic Mode: --periodic : Enables Periodic Mode
	
		5-2-1. Periodic Mode enables cells on the edge of the grid to look at
			neighbours on the opposite end of the grid.
		5-2-2. The default setting is Off.
	
	5-3. Random Factor: --random {probability} : Adjusts the probability a cell will be alive/dead
	
		5-3-1. A higher probability chance means that there will be more live cells in the grid
			whereas a lower chance means less live cells in the grid. 
		5-3-2. The probability must be between 0 and 1.
	
	5-4. Input File: --seed {path/to/filename} : Uses the seed file to generate live cells
	
		5-4-1. The file specified must have the correct file path and must have the '.seed' extension
	
	5-5. Generations: --generations {number} : Adjusts the number of generations the simulation will run for
	
		5-5-1. The default value is 50 generations.
		5-5-2. The value must be positive and not zero.
	
	5-6. Update Rate: --max-update {ups} : Adjusts the speed at which new generations update
	
		5-6-1. The default rate is 5 updates/sec.
		5-6-2. The value specified must be between 1 and 30.
	
	5-7. Step Mode: --step : Enables Step Mode
	
		5-7-1. Step mode allows the user to progress to the next generation by pressing Spacebar.
		5-7-2. The default setting is Off.
	
	5-8. Neighbourhood: --neighbour {type} {order} {centre-count}
	
		5-8-1. Type changes if how the neighbourhood will be observed.
			The two available types are "Moore" and "VonNeumann".
		5-8-2. Order changes how far away from the centre is being checked.
			The order must be between 1 and 10 and less than the smallest dimension.
		5-8-3. Centre-Count determines whether the centre cell being checked is counted as a live neighbour.
	
	5-9. Survival/Birth Rules: --survival {param1} {param2} --birth {param1} {param2}
	
		5-9-1. The survival/birth rules indicate the conditions required for cells to survive or be born.
		5-9-2. The rules take an arbitrary number of parameters. Of which needs to be greater than or equal to 0
		5-9-3. To enter a range of numbers, enter two integers separated by a "...". eg. 10...20
		5-9-4. The numbers provided must be less than or equal to the number of neighbouring cells and non-negative.
	
	5-10. Generational Memory: --memory {number} : The number of generations stored to detect steady state
	
		5-10-1. The number entered must be a integer between 4 and 512 (inclusive).
		5-11-1. The default number is 16.
	
	5-11. Output File: --output {filename}
	
		5-11-1. If an output file is specified, it will generate an output which contains the final generation of cells
		5-11-2. The file generated is formatted into a version 2 seed file.
		5-11-3. The filename specified must have a valid absolute or relative file path.
	
	5-12. Ghost Mode: --ghost : Enables ghost mode
	
		5-12-1. If ghost mode is enabled  then the most recent 4 generations of cells will be rendered with
			gradual reduction in colour strength.
		5-12-2. The default setting is Off.
...

## Notes 
1. Random generation of cells only works if there is no input file specified.
2. If step mode is On, then the update rate doesn't matter as the simulation only updates when the user presses Spacebar.
3. If a steady state is detected, the program will complete.
...
