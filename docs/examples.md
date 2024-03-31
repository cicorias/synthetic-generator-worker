# Examples using bash


```sh
#!/bin/bash

# Define the output file
output_file="command_output.log"

# Function to determine the mean value for each hour (example function)
get_mean_for_hour() {
    hour=$1
    # Example calculation for mean (customize as needed)
    echo $((hour * 5 + 10))
}

# Loop through all 24 hours of the prior day
for hour in {00..23}; do
    # Define start and end times for the current hour
    start_time=$(date -d "yesterday $hour:00" +%s)
    end_time=$(date -d "yesterday $hour:59:59" +%s)

    # Calculate the mean for the current hour
    mean=$(get_mean_for_hour $hour)

    # Run your command with start_time, end_time, and mean as parameters
    # Append the output to the file
    your_command --start "$start_time" --end "$end_time" --mean "$mean" >> "$output_file"
done

echo "Execution complete. Output appended to $output_file"
```

```sh
#!/bin/bash

# Define the output file
output_file="command_output.log"

# Path to the file containing mean values
mean_file="means.txt"

# Check if mean file exists
if [ ! -f "$mean_file" ]; then
    echo "Mean file not found: $mean_file"
    exit 1
fi

# Read the mean values into an array
mapfile -t mean_values < "$mean_file"

# Loop through all 24 hours of the prior day
for hour in {00..23}; do
    # Define start and end times for the current hour
    start_time=$(date -d "yesterday $hour:00" +%s)
    end_time=$(date -d "yesterday $hour:59:59" +%s)

    # Get the mean value for the current hour from the array
    mean=${mean_values[hour]}

    # Run your command with start_time, end_time, and mean as parameters
    # Append the output to the file
    your_command --start "$start_time" --end "$end_time" --mean "$mean" >> "$output_file"
done

echo "Execution complete. Output appended to $output_file"

```


```bash
#macos
#!/bin/bash

# Define the output file
output_file="command_output.log"

# Path to the file containing mean values
mean_file="means.txt"

# Check if mean file exists
if [ ! -f "$mean_file" ]; then
    echo "Mean file not found: $mean_file"
    exit 1
fi

# Initialize an empty array for mean values
mean_values=()

# Read the mean values into the array
while IFS= read -r line; do
    mean_values+=("$line")
done < "$mean_file"


# Loop through all 24 hours of the prior day
for hour in {00..02}; do
    # Define start and end times for the current hour
    # start_time=$(date -d "yesterday $hour:00" +%s)
    # end_time=$(date -d "yesterday $hour:59:59" +%s)

    start_time=$(date -v-1d -v"${hour}H" +%Y-%m-%dT%H:%M:%S)
    end_time=$(date -v-1d -v"${hour}H" -v"59M" -v"59S" +%s)

    # Get the mean value for the current hour from the array
    mean=${mean_values[hour]}

    # Run your command with start_time, end_time, and mean as parameters
    # Append the output to the file
    ./SyntheticGenerator --ReplayOrdersOptions:WindowStartTimeStr "$start_time" ----ReplayOrdersOptions:WindowStartTimeStr "$end_time" ----ReplayOrdersOptions:Lambda "$mean" >> "$output_file"
done

echo "Execution complete. Output appended to $output_file"

```