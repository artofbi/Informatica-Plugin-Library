################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
CPP_SRCS += \
../CheckSumSHA1.cpp 

OBJS += \
./CheckSumSHA1.o 

CPP_DEPS += \
./CheckSumSHA1.d 


# Each subdirectory must supply rules for building sources it contributes
%.o: ../%.cpp
	@echo 'Building file: $<'
	@echo 'Invoking: GCC C++ Compiler'
	g++ -I/home/DemoAdmin/INFA_DEV/_include -I/home/DemoAdmin/INFA_DEV/_include/hashlibpp_0_3_2/src -O0 -g3 -Wall -c -fmessage-length=0 -fPIC -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -o"$@" "$<"
	@echo 'Finished building: $<'
	@echo ' '


