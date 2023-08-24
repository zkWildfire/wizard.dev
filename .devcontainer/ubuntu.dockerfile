FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy
USER root

ARG USERNAME
ARG USER_UID
ARG USER_GID

ARG CLANG_VERSION=15
ARG CMAKE_VERSION=3.26.4
ARG CMAKE_VERSION_SHORT=3.26
ARG CMAKE_VERSION_NUMERIC=326
ARG GCC_VERSION=12
ARG PYTHON_VERSION=3.11

# Install apt packages
RUN apt-get update -y && \
	apt-get install -y \
	 	clang-${CLANG_VERSION} \
		curl \
		doxygen \
		git \
		g++-${GCC_VERSION} \
		libcairo2-dev \
		ninja-build \
		pkg-config \
		python${PYTHON_VERSION} \
		python${PYTHON_VERSION}-dev \
		python3-pip \
		ssh \
		sudo \
		tar \
		tree \
		unzip \
		valgrind \
		vim \
		zip && \
	update-alternatives --install \
		/usr/bin/python3 python3 /usr/bin/python${PYTHON_VERSION} 3 && \
	# Note - only clang needs to be set up using update-alternatives. A symlink
	#   from /usr/bin/g++ to the installed gcc version will be set up when GCC
	#   is installed.
	update-alternatives --install \
		/usr/bin/clang++ clang++ /usr/bin/clang++-${CLANG_VERSION} ${CLANG_VERSION}

# Install pip packages
COPY requirements.txt /tmp/requirements.txt
RUN python3 -m pip install -r /tmp/requirements.txt

# Add CMake
RUN mkdir -p /tmp/${USERNAME} && \
	wget -O /tmp/${USERNAME}/cmake.tar.gz \
		https://github.com/Kitware/CMake/releases/download/v${CMAKE_VERSION}/cmake-${CMAKE_VERSION}-linux-x86_64.tar.gz && \
	# Extract the files from the tarball to a temporary directory
	# This is done because the resulting directory structure after extracting
	#   the tarball will look like `/tmp/kymira/cmake/cmake-3.26.4-linux-etc/...`
	mkdir -p /tmp/${USERNAME}/cmake && \
	tar -xvf /tmp/${USERNAME}/cmake.tar.gz -C /tmp/${USERNAME}/cmake && \
	# Move the CMake files into the Tools folder
	mkdir -p /tools/cmake && \
	mv /tmp/${USERNAME}/cmake/cmake-${CMAKE_VERSION}-linux-x86_64 /tools/cmake/${CMAKE_VERSION} && \
	ln -s /tools/cmake/${CMAKE_VERSION}/bin/cmake /usr/bin/cmake${CMAKE_VERSION_SHORT} && \
	ln -s /tools/cmake/${CMAKE_VERSION}/bin/cpack /usr/bin/cpack${CMAKE_VERSION_SHORT} && \
	ln -s /tools/cmake/${CMAKE_VERSION}/bin/ctest /usr/bin/ctest${CMAKE_VERSION_SHORT} && \
	# Remove unnecessary files to keep container image size down
	rm -r /tools/cmake/${CMAKE_VERSION}/doc && \
	rm -r /tools/cmake/${CMAKE_VERSION}/man && \
	# Make CMake binaries available on the PATH
	update-alternatives --install \
		/usr/bin/cmake cmake /tools/cmake/${CMAKE_VERSION}/bin/cmake ${CMAKE_VERSION_NUMERIC} \
		--slave /usr/bin/cpack cpack /tools/cmake/${CMAKE_VERSION}/bin/cpack \
		--slave /usr/bin/ctest ctest /tools/cmake/${CMAKE_VERSION}/bin/ctest && \
	# Clean up temporary files
	rm -rf /tmp/${USERNAME}

# Generate the .NET developer certificate
RUN dotnet dev-certs https

# Create the user
RUN groupadd --gid $USER_GID $USERNAME && \
	useradd --uid $USER_UID --gid $USER_GID -m $USERNAME -s /bin/bash && \
	echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME && \
	chmod 0440 /etc/sudoers.d/$USERNAME
USER $USERNAME
COPY .vimrc /home/$USERNAME/.vimrc

# Add the path that pip scripts are added to
# Note that using `${PATH}` and `$PATH` may have different behavior according
#   to https://github.com/moby/moby/issues/42863. For safety, prefer `$PATH`.
ENV PATH="$PATH:/home/$USERNAME/.local/bin"
