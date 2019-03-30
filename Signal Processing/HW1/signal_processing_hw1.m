M2=5;
fl = 1:1:49; % Frequency range
for k = 1:length(fl)
f = fl(k); % Sine wave frequency
sim('signal_processing_simulink_hw1'); % The name of the simulink file
Av(k) = max(abs(y))/max(abs(x)); %Filter gain
end
plot(fl, Av)