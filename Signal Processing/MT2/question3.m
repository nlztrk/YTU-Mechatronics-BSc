%% Initialize parameters
W = 40;
Fs = 44100/W;
Ts = 1/Fs; 
N = 7; % The order of the filter

f = [0 1];       % Frequency band edges (as pi)
a = [0 pi*Fs];         % Desired amplitudes


%% Place your code here...
%  Design a FIR differentiator with Parks-Mccellan
%  Set bD as your filter coefficients

bD = firpm(N,f,a,'d'); % The coefficients of the filter

% Plot the frequency response of your filter
% y axis is magnitude (not in dB)
% x axis is frequency in Hz. Plot for the range [0 - Fs/2]
% Place your plot code here,

[H,W]=freqz(bD); % Getting the frequency and magnitude values from the frequency analysis
plot(W/(2*pi*Ts),abs(H)); % Drawing of the designed filter's response until the Fs/2
hold on;
plot([0 Fs/2], [0 pi*Fs], '--r'); % Drawing of the ideal filter's response
hold off;
legend('Filter', 'Ideal Differentiator', 'Location', 'NorthWest')

% End of your code - END OF EDIT
%% Plot the results - DON'T EDIT


% Print the coefficients
% clc
for k = 1:N+1
    fprintf('b(%d) = %8.6g, \n', k-1, bD(k));
end

% Clear needless variables and save the filter
clearvars -except bD
save differentiator_coefficients;