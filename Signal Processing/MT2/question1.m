%% Initialize parameters - DON'T EDIT
Fs = 44100; Ts = 1/Fs;

%% Place your code here
wp = 2*200/Fs; % Pass-band edge frequency
ws = 2*400/Fs; % Stop-band edge frequency

rp = 1;  % Maximum pass band ripple 1dB
rs = 40;   % Minimum stop band attenuation 40 dB

%  Set bL and aL as coefficients of your filter

[N,wc] = buttord(wp,ws,rp,rs); % Getting the cutoff frequency and order of the filter values
[bL,aL] = butter(N,wc); % Getting the filter coefficients

% Plot the frequency response in 2 subplots
% Subplot 1 - Plot the frequency between [0 - Fs/2] Hz.
% Subplot 2 - Plot the frequency between [0 - 600] Hz (Zoomed version).
% x axes are in terms of Hz.
% y axes are in terms of dBs.

figure(1)
subplot(2,1,1)
[h,w]=freqz(bL,aL); % Getting the frequency and magnitude values from frequency analysis
plot(w/(2*pi*Ts),mag2db(abs(h))); % Plotting the magnitude response
xlabel('Frequency (Hz)'); ylabel('Magnitude (dB)');
xlim([0,22050]); % Plotting for frequencies between 0-22050

subplot(2,1,2)
[h,w]=freqz(bL,aL); % Getting the frequency and magnitude values from frequency analysis
plot(w/(2*pi*Ts),mag2db(abs(h))); % Plotting the magnitude response
xlabel('Frequency (Hz)'); ylabel('Magnitude (dB)');
xlim([0,600]); % Plotting for frequencies between 0-600

% Find the transfer function of the filter.
% Plot the pole-zero map of the filter.

Ts = 1; % Sampling period
Hb = tf(bL, aL, Ts); % Creating the transfer function with coefficient values and the sampling constant

figure (2)
pzmap(Hb); % Getting the pole-zero map of the filter

% End of your code

%% Print the coefficients and save the results - DON'T EDIT
clc
for k = 1:N+1
    fprintf('a(%d) = %8.6g, \t b(%d) = %8.6g \n', k-1, aL(k),k-1, bL(k));
end

% Clear needless variables and save the filter
clearvars -except bL aL
save low_pass_coefficients;